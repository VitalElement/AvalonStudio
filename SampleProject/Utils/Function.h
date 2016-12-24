/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org/>
 */
// despite that it would be nice if you give credit to Malte Skarupke

#ifndef _function_h_
#define _function_h_

#include <utility>
#include <type_traits>
#include <functional>
#include <exception>
#include <typeinfo>
#include <memory>

#ifdef _MSC_VER
#define FUNC_NOEXCEPT
#define FUNC_TEMPLATE_NOEXCEPT(FUNCTOR, ALLOCATOR)
#define FUNC_CONSTEXPR const
#else
#define FUNC_NOEXCEPT noexcept
#define FUNC_TEMPLATE_NOEXCEPT(FUNCTOR, ALLOCATOR) \
    noexcept (detail::is_inplace_allocated<FUNCTOR, ALLOCATOR>::value)
#define FUNC_CONSTEXPR constexpr
#endif
#ifdef __GNUC__
#pragma GCC diagnostic push
#pragma GCC diagnostic ignored "-Wstrict-aliasing"
#endif

#define FUNC_MOVE(value)                                                    \
    static_cast<typename std::remove_reference<decltype (value)>::type&&> ( \
    value)
#define FUNC_FORWARD(type, value) static_cast<type&&> (value)
#define FUNC_NO_RTTI
#define FUNC_NO_EXCEPTIONS
namespace func
{
#ifndef FUNC_NO_EXCEPTIONS
    struct bad_function_call : std::exception
    {
        const char* what () const FUNC_NOEXCEPT override
        {
            return "Bad function call";
        }
    };
#endif

    template <typename> struct force_function_heap_allocation : std::false_type
    {
    };

    template <typename> class function;

    namespace detail
    {
        struct manager_storage_type;
        struct function_manager;
        struct functor_padding
        {
          protected:
            size_t padding_first;
            size_t padding_second;
        };

        struct empty_struct
        {
        };

#ifndef FUNC_NO_EXCEPTIONS
        template <typename Result, typename... Arguments>
        Result empty_call (const functor_padding&, Arguments...)
        {
            throw bad_function_call ();
        }
#endif

        template <typename T, typename Allocator> struct is_inplace_allocated
        {
            static const bool value
            // so that it fits
            = sizeof (T) <= sizeof (functor_padding)
              // so that it will be aligned
              &&
              std::alignment_of<functor_padding>::value %
              std::alignment_of<T>::value ==
              0
              // so that we can offer noexcept move
              &&
              std::is_nothrow_move_constructible<T>::value
              // so that the user can override it
              &&
              !force_function_heap_allocation<T>::value;
        };

        template <typename T> T to_functor (T&& func)
        {
            return FUNC_FORWARD (T, func);
        }
        template <typename Result, typename Class, typename... Arguments>
        auto to_functor (Result (Class::*func)(Arguments...))
        -> decltype (std::mem_fn (func))
        {
            return std::mem_fn (func);
        }
        template <typename Result, typename Class, typename... Arguments>
        auto to_functor (Result (Class::*func)(Arguments...) const)
        -> decltype (std::mem_fn (func))
        {
            return std::mem_fn (func);
        }

        template <typename T> struct functor_type
        {
            typedef decltype (to_functor (std::declval<T> ())) type;
        };

        template <typename T> bool is_null (const T&)
        {
            return false;
        }
        template <typename Result, typename... Arguments>
        bool is_null (Result (*const& function_pointer)(Arguments...))
        {
            return function_pointer == nullptr;
        }
        template <typename Result, typename Class, typename... Arguments>
        bool is_null (Result (Class::*const& function_pointer)(Arguments...))
        {
            return function_pointer == nullptr;
        }
        template <typename Result, typename Class, typename... Arguments>
        bool is_null (Result (Class::*const& function_pointer)(Arguments...)
                      const)
        {
            return function_pointer == nullptr;
        }

        template <typename, typename> struct is_valid_function_argument
        {
            static const bool value = false;
        };

        template <typename Result, typename... Arguments>
        struct is_valid_function_argument<function<Result (Arguments...)>,
                                          Result (Arguments...)>
        {
            static const bool value = false;
        };

        template <typename T, typename Result, typename... Arguments>
        struct is_valid_function_argument<T, Result (Arguments...)>
        {
#ifdef _MSC_VER
            // as of january 2013 visual studio doesn't support the SFINAE below
            static const bool value = true;
#else
            template <typename U>
            static decltype (
            to_functor (std::declval<U> ())(std::declval<Arguments> ()...))
            check (U*);
            template <typename> static empty_struct check (...);

            static const bool value =
            std::is_convertible<decltype (check<T> (nullptr)), Result>::value;
#endif
        };

        typedef const function_manager* manager_type;

        struct manager_storage_type
        {
            template <typename Allocator>
            Allocator& get_allocator () FUNC_NOEXCEPT
            {
                return reinterpret_cast<Allocator&> (manager);
            }
            template <typename Allocator>
            const Allocator& get_allocator () const FUNC_NOEXCEPT
            {
                return reinterpret_cast<const Allocator&> (manager);
            }

            functor_padding functor;
            manager_type manager;
        };

        template <typename T, typename Allocator, typename Enable = void>
        struct function_manager_inplace_specialization
        {
            template <typename Result, typename... Arguments>
            static Result call (const functor_padding& storage,
                                Arguments... arguments)
            {
                // do not call get_functor_ref because I want this function to
                // be fast
                // in debug when nothing gets inlined
                return const_cast<T&> (reinterpret_cast<const T&> (storage))(
                FUNC_FORWARD (Arguments, arguments)...);
            }

            static void store_functor (manager_storage_type& storage,
                                       T to_store)
            {
                new (&get_functor_ref (storage)) T (FUNC_FORWARD (T, to_store));
            }
            static void move_functor (manager_storage_type& lhs,
                                      manager_storage_type&& rhs) FUNC_NOEXCEPT
            {
                new (&get_functor_ref (lhs))
                T (FUNC_MOVE (get_functor_ref (rhs)));
            }
            static void
            destroy_functor (Allocator&,
                             manager_storage_type& storage) FUNC_NOEXCEPT
            {
                get_functor_ref (storage).~T ();
            }
            static T&
            get_functor_ref (const manager_storage_type& storage) FUNC_NOEXCEPT
            {
                return const_cast<T&> (
                reinterpret_cast<const T&> (storage.functor));
            }
        };
        template <typename T, typename Allocator>
        struct function_manager_inplace_specialization<
        T, Allocator, typename std::enable_if<
                      !is_inplace_allocated<T, Allocator>::value>::type>
        {
            template <typename Result, typename... Arguments>
            static Result call (const functor_padding& storage,
                                Arguments... arguments)
            {
                // do not call get_functor_ptr_ref because I want this function
                // to be fast
                // in debug when nothing gets inlined
                return (
                *reinterpret_cast<
                const typename std::allocator_traits<Allocator>::pointer&> (
                storage))(FUNC_FORWARD (Arguments, arguments)...);
            }

            static void store_functor (manager_storage_type& self, T to_store)
            {
                Allocator& allocator = self.get_allocator<Allocator> ();
                ;
                static_assert (
                sizeof (typename std::allocator_traits<Allocator>::pointer) <=
                sizeof (self.functor),
                "The allocator's pointer type is too big");
                typename std::allocator_traits<Allocator>::pointer* ptr =
                new (&get_functor_ptr_ref (self))
                typename std::allocator_traits<Allocator>::pointer (
                std::allocator_traits<Allocator>::allocate (allocator, 1));
                std::allocator_traits<Allocator>::construct (
                allocator, *ptr, FUNC_FORWARD (T, to_store));
            }
            static void move_functor (manager_storage_type& lhs,
                                      manager_storage_type&& rhs) FUNC_NOEXCEPT
            {
                static_assert (
                std::is_nothrow_move_constructible<
                typename std::allocator_traits<Allocator>::pointer>::value,
                "we can't offer a noexcept swap if the pointer type is not "
                "nothrow move constructible");
                new (&get_functor_ptr_ref (lhs))
                typename std::allocator_traits<Allocator>::pointer (
                FUNC_MOVE (get_functor_ptr_ref (rhs)));
                // this next assignment makes the destroy function easier
                get_functor_ptr_ref (rhs) = nullptr;
            }
            static void
            destroy_functor (Allocator& allocator,
                             manager_storage_type& storage) FUNC_NOEXCEPT
            {
                typename std::allocator_traits<Allocator>::pointer& pointer =
                get_functor_ptr_ref (storage);
                if (!pointer)
                    return;
                std::allocator_traits<Allocator>::destroy (allocator, pointer);
                std::allocator_traits<Allocator>::deallocate (allocator,
                                                              pointer, 1);
            }
            static T&
            get_functor_ref (const manager_storage_type& storage) FUNC_NOEXCEPT
            {
                return *get_functor_ptr_ref (storage);
            }
            static typename std::allocator_traits<Allocator>::pointer&
            get_functor_ptr_ref (manager_storage_type& storage) FUNC_NOEXCEPT
            {
                return reinterpret_cast<
                typename std::allocator_traits<Allocator>::pointer&> (
                storage.functor);
            }
            static const typename std::allocator_traits<Allocator>::pointer&
            get_functor_ptr_ref (const manager_storage_type& storage)
            FUNC_NOEXCEPT
            {
                return reinterpret_cast<
                const typename std::allocator_traits<Allocator>::pointer&> (
                storage.functor);
            }
        };

        template <typename T, typename Allocator>
        static const function_manager& get_default_manager ();

        template <typename T, typename Allocator>
        static void create_manager (manager_storage_type& storage,
                                    Allocator&& allocator)
        {
            new (&storage.get_allocator<Allocator> ())
            Allocator (FUNC_MOVE (allocator));
            storage.manager = &get_default_manager<T, Allocator> ();
        }

        // this struct acts as a vtable. it is an optimization to prevent
        // code-bloat from rtti. see the documentation of boost::function
        struct function_manager
        {
            template <typename T, typename Allocator>
            inline static FUNC_CONSTEXPR function_manager
            create_default_manager ()
            {
#ifdef _MSC_VER
                function_manager result =
#else
                return function_manager
#endif
                { &templated_call_move_and_destroy<T, Allocator>,
                  &templated_call_copy<T, Allocator>,
                  &templated_call_copy_functor_only<T, Allocator>,
                  &templated_call_destroy<T, Allocator>,
#ifndef FUNC_NO_RTTI
                  &templated_call_type_id<T, Allocator>,
                  &templated_call_target<T, Allocator>
#endif
                };
#ifdef _MSC_VER
                return result;
#endif
            }

            void (*const call_move_and_destroy)(manager_storage_type& lhs,
                                                manager_storage_type&& rhs);
            void (*const call_copy)(manager_storage_type& lhs,
                                    const manager_storage_type& rhs);
            void (*const call_copy_functor_only)(
            manager_storage_type& lhs, const manager_storage_type& rhs);
            void (*const call_destroy)(manager_storage_type& manager);
#ifndef FUNC_NO_RTTI
            const std::type_info& (*const call_type_id)();
            void* (*const call_target)(const manager_storage_type& manager,
                                       const std::type_info& type);
#endif

            template <typename T, typename Allocator>
            static void
            templated_call_move_and_destroy (manager_storage_type& lhs,
                                             manager_storage_type&& rhs)
            {
                typedef function_manager_inplace_specialization<T, Allocator>
                specialization;
                specialization::move_functor (lhs, FUNC_MOVE (rhs));
                specialization::destroy_functor (
                rhs.get_allocator<Allocator> (), rhs);
                create_manager<T, Allocator> (
                lhs, FUNC_MOVE (rhs.get_allocator<Allocator> ()));
                rhs.get_allocator<Allocator> ().~Allocator ();
            }
            template <typename T, typename Allocator>
            static void templated_call_copy (manager_storage_type& lhs,
                                             const manager_storage_type& rhs)
            {
                typedef function_manager_inplace_specialization<T, Allocator>
                specialization;
                create_manager<T, Allocator> (
                lhs, Allocator (rhs.get_allocator<Allocator> ()));
                specialization::store_functor (
                lhs, specialization::get_functor_ref (rhs));
            }
            template <typename T, typename Allocator>
            static void templated_call_destroy (manager_storage_type& self)
            {
                typedef function_manager_inplace_specialization<T, Allocator>
                specialization;
                specialization::destroy_functor (
                self.get_allocator<Allocator> (), self);
                self.get_allocator<Allocator> ().~Allocator ();
            }
            template <typename T, typename Allocator>
            static void
            templated_call_copy_functor_only (manager_storage_type& lhs,
                                              const manager_storage_type& rhs)
            {
                typedef function_manager_inplace_specialization<T, Allocator>
                specialization;
                specialization::store_functor (
                lhs, specialization::get_functor_ref (rhs));
            }
#ifndef FUNC_NO_RTTI
            template <typename T, typename>
            static const std::type_info& templated_call_type_id ()
            {
                return typeid (T);
            }
            template <typename T, typename Allocator>
            static void*
            templated_call_target (const manager_storage_type& self,
                                   const std::type_info& type)
            {
                typedef function_manager_inplace_specialization<T, Allocator>
                specialization;
                if (typedef == typeid (T))
                    return &specialization::get_functor_ref (self);
                else
                    return nullptr;
            }
#endif
        };
        template <typename T, typename Allocator>
        inline static const function_manager& get_default_manager ()
        {
            static FUNC_CONSTEXPR function_manager default_manager =
            function_manager::create_default_manager<T, Allocator> ();
            return default_manager;
        }

        template <typename Result, typename...> struct typedeffer
        {
            typedef Result result_type;
        };
        template <typename Result, typename Argument>
        struct typedeffer<Result, Argument>
        {
            typedef Result result_type;
            typedef Argument argument_type;
        };
        template <typename Result, typename First_Argument,
                  typename Second_Argument>
        struct typedeffer<Result, First_Argument, Second_Argument>
        {
            typedef Result result_type;
            typedef First_Argument first_argument_type;
            typedef Second_Argument second_argument_type;
        };
    }

    template <typename Result, typename... Arguments>
    class function<Result (Arguments...)>
    : public detail::typedeffer<Result, Arguments...>
    {
      public:
        function () FUNC_NOEXCEPT
        {
            initialize_empty ();
        }
        function (std::nullptr_t) FUNC_NOEXCEPT
        {
            initialize_empty ();
        }
        function (function&& other) FUNC_NOEXCEPT
        {
            initialize_empty ();
            swap (other);
        }
        function (const function& other) : call (other.call)
        {
            other.manager_storage.manager->call_copy (manager_storage,
                                                      other.manager_storage);
        }
        template <typename T>
        function (
        T functor,
        typename std::enable_if<
        detail::is_valid_function_argument<T, Result (Arguments...)>::value,
        detail::empty_struct>::type = detail::empty_struct ())
        FUNC_TEMPLATE_NOEXCEPT (
        T, std::allocator<typename detail::functor_type<T>::type>)
        {
            if (detail::is_null (functor))
            {
                initialize_empty ();
            }
            else
            {
                typedef typename detail::functor_type<T>::type functor_type;
                initialize (detail::to_functor (FUNC_FORWARD (T, functor)),
                            std::allocator<functor_type> ());
            }
        }
        template <typename Allocator>
        function (std::allocator_arg_t, const Allocator&)
        {
            // ignore the allocator because I don't allocate
            initialize_empty ();
        }
        template <typename Allocator>
        function (std::allocator_arg_t, const Allocator&, std::nullptr_t)
        {
            // ignore the allocator because I don't allocate
            initialize_empty ();
        }
        template <typename Allocator, typename T>
        function (
        std::allocator_arg_t, const Allocator& allocator, T functor,
        typename std::enable_if<
        detail::is_valid_function_argument<T, Result (Arguments...)>::value,
        detail::empty_struct>::type = detail::empty_struct ())
        FUNC_TEMPLATE_NOEXCEPT (T, Allocator)
        {
            if (detail::is_null (functor))
            {
                initialize_empty ();
            }
            else
            {
                initialize (detail::to_functor (FUNC_FORWARD (T, functor)),
                            Allocator (allocator));
            }
        }
        template <typename Allocator>
        function (std::allocator_arg_t, const Allocator& allocator,
                  const function& other)
            : call (other.call)
        {
            typedef typename std::allocator_traits<
            Allocator>::template rebind_alloc<function> MyAllocator;

            // first try to see if the allocator matches the target type
            detail::manager_type manager_for_allocator =
            &detail::get_default_manager<
            typename std::allocator_traits<Allocator>::value_type,
            Allocator> ();
            if (other.manager_storage.manager == manager_for_allocator)
            {
                detail::create_manager<
                typename std::allocator_traits<Allocator>::value_type,
                Allocator> (manager_storage, Allocator (allocator));
                manager_for_allocator->call_copy_functor_only (
                manager_storage, other.manager_storage);
            }
            // if it does not, try to see if the target contains my type. this
            // breaks the recursion of the last case. otherwise repeated copies
            // would allocate more and more memory
            else
            {
                detail::manager_type manager_for_function =
                &detail::get_default_manager<function, MyAllocator> ();
                if (other.manager_storage.manager == manager_for_function)
                {
                    detail::create_manager<function, MyAllocator> (
                    manager_storage, MyAllocator (allocator));
                    manager_for_function->call_copy_functor_only (
                    manager_storage, other.manager_storage);
                }
                else
                {
                    // else store the other function as my target
                    initialize (other, MyAllocator (allocator));
                }
            }
        }
        template <typename Allocator>
        function (std::allocator_arg_t, const Allocator&,
                  function&& other) FUNC_NOEXCEPT
        {
            // ignore the allocator because I don't allocate
            initialize_empty ();
            swap (other);
        }

        function& operator=(function other) FUNC_NOEXCEPT
        {
            swap (other);
            return *this;
        }
        ~function () FUNC_NOEXCEPT
        {
            manager_storage.manager->call_destroy (manager_storage);
        }

        Result operator()(Arguments... arguments) const
        {
            return call (manager_storage.functor,
                         FUNC_FORWARD (Arguments, arguments)...);
        }

        template <typename T, typename Allocator>
        void assign (T&& functor, const Allocator& allocator)
        FUNC_TEMPLATE_NOEXCEPT (T, Allocator)
        {
            function (std::allocator_arg, allocator, functor).swap (*this);
        }

        void swap (function& other) FUNC_NOEXCEPT
        {
            detail::manager_storage_type temp_storage;
            other.manager_storage.manager->call_move_and_destroy (
            temp_storage, FUNC_MOVE (other.manager_storage));
            manager_storage.manager->call_move_and_destroy (
            other.manager_storage, FUNC_MOVE (manager_storage));
            temp_storage.manager->call_move_and_destroy (
            manager_storage, FUNC_MOVE (temp_storage));

            std::swap (call, other.call);
        }


#ifndef FUNC_NO_RTTI
        const std::type_info& target_type () const FUNC_NOEXCEPT
        {
            return manager_storage.manager->call_type_id ();
        }
        template <typename T> T* target () FUNC_NOEXCEPT
        {
            return static_cast<T*> (
            manager_storage.manager->call_target (manager_storage, typeid (T)));
        }
        template <typename T> const T* target () const FUNC_NOEXCEPT
        {
            return static_cast<const T*> (
            manager_storage.manager->call_target (manager_storage, typeid (T)));
        }
#endif

        operator bool() const FUNC_NOEXCEPT
        {

#ifdef FUNC_NO_EXCEPTIONS
            return call != nullptr;
#else
            return call != &detail::empty_call<Result, Arguments...>;
#endif
        }

        bool operator==(const function& other) const FUNC_NOEXCEPT
        {
            return call == other.call;
        }

        // private:
        detail::manager_storage_type manager_storage;
        Result (*call)(const detail::functor_padding&, Arguments...);

        template <typename T, typename Allocator>
        void initialize (T functor, Allocator&& allocator)
        {
            call = &detail::function_manager_inplace_specialization<
                   T, Allocator>::template call<Result, Arguments...>;
            detail::create_manager<T, Allocator> (
            manager_storage, FUNC_FORWARD (Allocator, allocator));
            detail::function_manager_inplace_specialization<
            T, Allocator>::store_functor (manager_storage,
                                          FUNC_FORWARD (T, functor));
        }

        typedef Result (*Empty_Function_Type)(Arguments...);
        void initialize_empty () FUNC_NOEXCEPT
        {
            typedef std::allocator<Empty_Function_Type> Allocator;
            static_assert (
            detail::is_inplace_allocated<Empty_Function_Type, Allocator>::value,
            "The empty function should benefit from small functor "
            "optimization");

            detail::create_manager<Empty_Function_Type, Allocator> (
            manager_storage, Allocator ());
            detail::function_manager_inplace_specialization<
            Empty_Function_Type, Allocator>::store_functor (manager_storage,
                                                            nullptr);
#ifdef FUNC_NO_EXCEPTIONS
            call = nullptr;
#else
            call = &detail::empty_call<Result, Arguments...>;
#endif
        }
    };

    template <typename T>
    bool operator==(std::nullptr_t, const function<T>& rhs) FUNC_NOEXCEPT
    {
        return !rhs;
    }
    template <typename T>
    bool operator==(const function<T>& lhs, std::nullptr_t) FUNC_NOEXCEPT
    {
        return !lhs;
    }
    template <typename T>
    bool operator!=(std::nullptr_t, const function<T>& rhs) FUNC_NOEXCEPT
    {
        return rhs;
    }
    template <typename T>
    bool operator!=(const function<T>& lhs, std::nullptr_t) FUNC_NOEXCEPT
    {
        return lhs;
    }

    template <typename T> void swap (function<T>& lhs, function<T>& rhs)
    {
        lhs.swap (rhs);
    }

} // end namespace func

namespace std
{
    template <typename Result, typename... Arguments, typename Allocator>
    struct uses_allocator<func::function<Result (Arguments...)>, Allocator>
    : std::true_type
    {
    };
}

#ifdef __GNUC__
#pragma GCC diagnostic pop
#endif
#undef FUNC_NOEXCEPT
#undef FUNC_TEMPLATE_NOEXCEPT
#undef FUNC_FORWARD
#undef FUNC_MOVE
#undef FUNC_CONSTEXPR

#endif
