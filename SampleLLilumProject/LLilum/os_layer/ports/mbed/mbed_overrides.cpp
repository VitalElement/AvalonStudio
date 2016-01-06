//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 

//--//

extern "C"
{
    //
    // NOTE: If you would like to configure your own MAC address, you can do so by
    // uncommenting the function below
    //

    //void mbed_mac_address(char *mac)
    //{
    //    mac[0] = (char)122;
    //    mac[1] = (char)71;
    //    mac[2] = (char)10;
    //    mac[3] = (char)135;
    //    mac[4] = (char)218;
    //    mac[5] = (char)0;

    //    // We want to force bits [1:0] of the most significant byte [0]
    //    // to be "10" 
    //    // http://en.wikipedia.org/wiki/MAC_address

    //    mac[0] |= 0x02; // force bit 1 to a "1" = "Locally Administered"
    //    mac[0] &= 0xFE; // force bit 0 to a "0" = Unicast
    //}
}
