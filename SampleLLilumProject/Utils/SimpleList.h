/******************************************************************************
*       Description:
*
*       Author:
*         Date: 11 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _LIST_H_
#define _LIST_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

/**
 * A double linked list class. Any type can inherit from this to become a list.
 * this implementation is extremely light with no checking, but allows extremly
 * efficient operation. Avoiding the necessity of dynamic memory allocation.
 */
class List
{
  public:
    /**
     * Constructs an instance of the Lists head.
     */
    List ()
    {
        this->next = this;
        this->prev = this;
    }

    /**
     * Adds an item to the head of the list.
     * @param item - pointer to the item to add.
     */
    void Add (List* item)
    {
        List::Add (item, this, this->next);
    }

    /**
     * Adds an item to the end or tail of the list.
     * @param item - pointer to the item to add.
     */
    void AddTail (List* item)
    {
        List::Add (item, this->prev, this);
    }

    /**
     * Removes an item from the list.
     * @param item - pointer to the item to remove.
     */
    void Remove (List* item)
    {
        List::Remove (item->prev, item->next);
    }

    /**
     * Pops an item from the list.
     * @return pointer to the item that has been popped, or nullptr if the list
     * is empty.
     */
    List* Pop ()
    {
        if (this->next == this)
        {
            return NULL;
        }

        List* item = this->next;
        Remove (item);

        return item;
    }

    /**
     * Pushes and item to the list. This is effectively the same as AddTail.
     * @param item - a poionter to the item to add to the list.
     */
    void Push (List* item)
    {
        this->AddTail (item);
    }

    List* prev;
    List* next;

  private:
    static void Add (List* newItem, List* prev, List* next)
    {
        next->prev = newItem;
        newItem->next = next;
        newItem->prev = prev;
        prev->next = newItem;
    }

    static void Remove (List* prev, List* next)
    {
        next->prev = prev;
        prev->next = next;
    }
};

#endif
