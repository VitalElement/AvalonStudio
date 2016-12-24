#ifndef _CIRCULARBUFFER_H_
#define _CIRCULARBUFFER_H_

#include <stdint.h>
template <class T> class CircularBuffer
{
  public:
    CircularBuffer (T* buffer, uint32_t size)
    {
        this->size = size;
        start = 0;
        end = 0;
        count = 0;
        elems = buffer;
    }

    ~CircularBuffer ()
    {
    }

    void Clear ()
    {
        start = 0;
        end = 0;
        count = 0;        
    }

    bool IsFull ()
    {
        return IncrementIndex (&end) == start;
    }

    bool IsEmpty ()
    {
        return end == start;
    }

    void Write (T data)
    {
        elems[end] = data;

        end = IncrementIndex (&end);

        if (end == start)
        {
            start = IncrementIndex (&start);
        }

        count++;
    }

    T Read ()
    {
        T result = elems[start];

        start = IncrementIndex (&start);
        count--;

        return result;
    }

    T* elems;
    uint32_t count;

  private:
    uint32_t IncrementIndex (uint32_t* index)
    {
        return (*index + 1) % size;
    }

    uint32_t size;
    uint32_t start;
    uint32_t end;
};

template <class T> class PointerCircularBuffer
{
  public:
    PointerCircularBuffer (T* buffer, uint32_t size)
    {
        this->size = size;
        start = 0;
        end = 0;
        count = 0;
        elems = buffer;
    }

    ~PointerCircularBuffer ()
    {
    }

    void Clear ()
    {
        start = 0;
        end = 0;
        count = 0;        
    }

    bool IsFull ()
    {
        return IncrementIndex (&end) == start;
    }

    bool IsEmpty ()
    {
        return end == start;
    }

    void Write (T* data)
    {
        elems[end] = data;

        end = IncrementIndex (&end);

        if (end == start)
        {
            start = IncrementIndex (&start);
        }

        count++;
    }

    T* Read ()
    {
        T result = elems[start];

        start = IncrementIndex (&start);
        count--;

        return &result;
    }

    T* elems;
    uint32_t count;

  private:
    uint32_t IncrementIndex (uint32_t* index)
    {
        return (*index + 1) % size;
    }

    uint32_t size;
    uint32_t start;
    uint32_t end;
};

#endif
