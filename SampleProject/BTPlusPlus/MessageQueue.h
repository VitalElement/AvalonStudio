/******************************************************************************
*       Description:
*
*       Author:
*         Date: 07 October 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _MESSAGE_QUEUE_H_
#define _MESSAGE_QUEUE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

#include "FreeRtos.h"
#include "queue.h"

template <typename T> class MessageQueue
{
#pragma mark Public Members
  public:
    MessageQueue ()
    {
    }

    ~MessageQueue ()
    {
    }

    static MessageQueue* Create (uint32_t length)
    {
        MessageQueue* result = new MessageQueue<T> ();

        result->handle = xQueueCreate (length, sizeof (T));

        return result;
    }

    bool Receive (T* buffer, uint32_t timeoutTicks)
    {
        return xQueueReceive (this->handle, buffer, timeoutTicks);
    }


    bool Send (T message, uint32_t timeoutTicks)
    {
        return xQueueSend (this->handle, &message, timeoutTicks);
    }


#pragma mark Private Members
  private:
    void* handle;
};

#endif
