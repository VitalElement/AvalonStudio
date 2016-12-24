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
#include "netdb.h"
#include "tcp.h"
#include "Sockets.h"

extern "C"
{
    int32_t LLOS_lwip_socket(int32_t family, int32_t type, int32_t protocol)
    {
        switch (protocol)
        {
        case 6://SOCK_IPPROTO_TCP:
            protocol = 6;// IPPROTO_TCP;
            break;
        case 17://SOCK_IPPROTO_UDP:
            protocol = 17;// IPPROTO_UDP;
            break;
        case 1://SOCK_IPPROTO_ICMP:
            protocol = 1;// IP_PROTO_ICMP;
            break;

        case 2://SOCK_IPPROTO_IGMP:
            protocol = 2;// IP_PROTO_IGMP;
            break;
        }

        return lwip_socket(family, type, protocol);
    }


    int32_t LLOS_lwip_bind(int32_t socket, void* address)
    {
        sockaddr_in _remoteHost;

        return lwip_bind(socket, (const struct sockaddr *) &_remoteHost, sizeof(_remoteHost));
    }


    int32_t LLOS_lwip_connect(int32_t socket, void* address, bool fThrowOnWouldBlock)
    {
        sockaddr_in _remoteHost;

        uint8_t p1 = ((uint8_t*)address)[2];
        uint8_t p2 = ((uint8_t*)address)[3];
        uint16_t port = p1 << 8 | p2;

        std::memcpy((char*)&_remoteHost.sin_addr.s_addr, &((uint8_t*)address)[4], 4);

        // Address family
        _remoteHost.sin_family = AF_INET;

        // Set port
        _remoteHost.sin_port = htons(port);

        return lwip_connect(socket, (const struct sockaddr *) &_remoteHost, sizeof(_remoteHost));
    }


    int32_t LLOS_lwip_send(int32_t socket, char* buf, int32_t count, int32_t flags, int32_t time_ms)
    {
        return lwip_send(socket, buf, count, flags);
    }


    int32_t LLOS_lwip_recv(int32_t socket, char* buf, int32_t count, int32_t flags, int32_t time_ms)
    {
        return lwip_recv(socket, buf, count, flags);
    }


    int32_t LLOS_lwip_close(int32_t socket)
    {
        return lwip_close(socket);
    }


    int32_t LLOS_lwip_listen(int32_t socket, int32_t backlog)
    {
        return lwip_listen(socket, backlog);
    }


    int32_t LLOS_lwip_accept(int32_t socket, void* address, uint32_t* addrlen)
    {
        return lwip_accept(socket, (sockaddr*)address, addrlen);
    }

    int32_t LLOS_lwip_getaddrinfo(uint16_t* name, uint32_t namelen, char* canonicalName, uint32_t canonicalNameBufferSize, char* addresses, uint32_t addressBufferSize)
    {
        char nameBuffer[256];

        if (namelen >= 256)
        {
            return ERR_ARG;
        }

        WStringToCharBuffer(nameBuffer, 256, name, namelen);
        nameBuffer[namelen] = '\0';

        addrinfo* addressInfo = NULL;
        int32_t res = lwip_getaddrinfo(nameBuffer, NULL, NULL, &addressInfo);

        char* canonName = addressInfo->ai_canonname;
        sockaddr* sockAddress = addressInfo->ai_addr;

        while (*canonName != '\0' && canonicalNameBufferSize > 0)
        {
            *canonicalName = *canonName;
            canonName++;
            canonicalName++;
            canonicalNameBufferSize--;
        }
        
        if (sockAddress->sa_len > addressBufferSize)
        {
            return ERR_VAL;
        }

        std::memcpy(addresses, sockAddress, sockAddress->sa_len);

        return res;
    }

    int32_t LLOS_lwip_shutdown(int32_t socket, int32_t how)
    {
        return lwip_shutdown(socket, how);
    }


    int32_t LLOS_lwip_sendto(int32_t socket, char* buf, int32_t count, int32_t flags, int32_t time_ms, void* address, uint32_t tolen)
    {
        return lwip_sendto(socket, buf, count, flags, (sockaddr*)address, tolen);
    }


    int32_t LLOS_lwip_recvfrom(int32_t socket, char* buf, int32_t count, int32_t flags, int32_t time_ms, void* address, uint32_t* fromlen)
    {
        return lwip_recvfrom(socket, buf, count, flags, (sockaddr*)address, fromlen);
    }


    int32_t LLOS_lwip_getpeername(int32_t socket, void* buf, uint32_t* namelen)
    {
        return lwip_getpeername(socket, (sockaddr*)buf, namelen);
    }


    int32_t LLOS_lwip_getsockname(int32_t socket, void* buf, uint32_t* namelen)
    {
        return lwip_getsockname(socket, (sockaddr*)buf, namelen);
    }


    int32_t LLOS_lwip_getsockopt(int32_t socket, int32_t level, int32_t optname, char* buf, uint32_t* optlen)
    {
        return lwip_getsockopt(socket, level, optname, buf, optlen);
    }


    int32_t LLOS_lwip_setsockopt(int32_t socket, int32_t level, int32_t optname, char* buf, uint32_t optlen)
    {
        return lwip_setsockopt(socket, level, optname, buf, optlen);
    }

    // TODO
    bool LLOS_lwip_poll(int32_t socket, int32_t mode, int32_t microSeconds)
    {
        return true;
    }


    int32_t LLOS_lwip_ioctl(int32_t socket, uint32_t cmd, uint32_t* arg)
    {
        return lwip_ioctl(socket, (long)cmd, arg);
    }
}
