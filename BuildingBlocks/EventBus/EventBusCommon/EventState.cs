// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT

// Framework code of microservices and domain drive design pattern

namespace EventBusCommon
{
    public enum EventState
    {
        NotPublished = 0,
        InProcess = 1,
        Published = 2,
        PublishedFailed = 3
    }
}
