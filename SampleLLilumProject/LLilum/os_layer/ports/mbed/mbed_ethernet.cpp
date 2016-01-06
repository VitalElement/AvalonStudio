//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h"
#include "mbed.h"
#include "EthernetInterface.h"
#include "init.h"
#include "tcpip.h"
#include "dns.h"
#include "netifapi.h"
#include "Netdb.h"
#include "tcp.h"
#include "Sockets.h"
#include "llos_error.h"

int32_t WStringToCharBuffer(char* output, uint32_t outputBufferLength, const uint16_t* input, const uint32_t length)
{
    if (length > outputBufferLength)
    {
        return -1;
    }

    uint32_t i;
    for (i = 0; i < length; i++)
    {
        uint16_t ch = input[i];
        output[i] = (ch > 0xFF) ? '?' : (char)ch;
    }

    return i;
}

extern "C"
{
#define MAXADDRSTRINGSIZE 32

    uint32_t LLOS_ethernet_address_from_string(uint16_t* address, uint32_t addrlen)
    {
        char ipBuffer[MAXADDRSTRINGSIZE];
        char tempAddress[5];
        uint32_t result = 0;

        if (WStringToCharBuffer(ipBuffer, MAXADDRSTRINGSIZE, address, addrlen) < 0)
        {
            return -1;
        }

        // Dot-decimal notation
        int scanResult = std::sscanf(ipBuffer, "%3u.%3u.%3u.%3u",
            (unsigned int*)&tempAddress[0],
            (unsigned int*)&tempAddress[1],
            (unsigned int*)&tempAddress[2],
            (unsigned int*)&tempAddress[3]);

        if (scanResult == 4)
        {
            result |= tempAddress[3] << 24;
            result |= tempAddress[2] << 16;
            result |= tempAddress[1] << 8;
            result |= tempAddress[0];
        }
        return result;
    }

    HRESULT LLOS_ethernet_dhcp_init()
    {
        return EthernetInterface::init();
    }

    HRESULT LLOS_ethernet_staticIP_init(const uint16_t* ip, const uint32_t ipLen, const uint16_t* mask, const uint32_t maskLen, const uint16_t* gateway, const uint32_t gatewayLen)
    {
        char ipBuffer[MAXADDRSTRINGSIZE];
        char maskBuffer[MAXADDRSTRINGSIZE];
        char gatewayBuffer[MAXADDRSTRINGSIZE];

        if (ip == NULL || mask == NULL || gateway == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (ipLen < MAXADDRSTRINGSIZE && maskLen < MAXADDRSTRINGSIZE && gatewayLen < MAXADDRSTRINGSIZE)
        {
            WStringToCharBuffer(ipBuffer, MAXADDRSTRINGSIZE, ip, ipLen);
            WStringToCharBuffer(maskBuffer, MAXADDRSTRINGSIZE, mask, maskLen);
            WStringToCharBuffer(gatewayBuffer, MAXADDRSTRINGSIZE, gateway, gatewayLen);

            ipBuffer[ipLen] = '\0';
            maskBuffer[maskLen] = '\0';
            gatewayBuffer[gatewayLen] = '\0';

            return EthernetInterface::init(ipBuffer, maskBuffer, gatewayBuffer);
        }

        return LLOS_E_INVALID_PARAMETER;
    }

    HRESULT LLOS_ethernet_connect(uint32_t timeout)
    {
        return EthernetInterface::connect(timeout);
    }

    HRESULT LLOS_ethernet_disconnect()
    {
        return EthernetInterface::disconnect();
    }

    HRESULT LLOS_ethernet_get_macAddress(uint16_t* address, uint32_t bufferLen)
    {
        if (address == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        char* temp = EthernetInterface::getMACAddress();
        while (*temp != '\0' && bufferLen > 0)
        {
            *address = *temp & 0x00FF;
            address++;
            temp++;
            bufferLen--;
        }

        return S_OK;
    }

    HRESULT LLOS_ethernet_get_IPv4Address(uint16_t* address, uint32_t bufferLen)
    {
        if (address == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        char* temp = EthernetInterface::getIPAddress();
        while (*temp != '\0' && bufferLen > 0)
        {
            *address = *temp & 0x00FF;
            address++;
            temp++;
            bufferLen--;
        }

        return S_OK;
    }

    HRESULT LLOS_ethernet_get_gatewayIPv4Address(uint16_t* address, uint32_t bufferLen)
    {
        if (address == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        char* temp = EthernetInterface::getGateway();
        while (*temp != '\0' && bufferLen > 0)
        {
            *address = *temp & 0x00FF;
            address++;
            temp++;
            bufferLen--;
        }

        return S_OK;
    }

    HRESULT LLOS_ethernet_get_networkIPv4Mask(uint16_t* mask, uint32_t bufferLen)
    {
        if (mask == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        char* temp = EthernetInterface::getNetworkMask();
        while (*temp != '\0' && bufferLen > 0)
        {
            *mask = *temp & 0x00FF;
            mask++;
            temp++;
            bufferLen--;
        }

        return S_OK;
    }
}
