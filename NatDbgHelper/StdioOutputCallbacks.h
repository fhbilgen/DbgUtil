#pragma once
#include <DbgEng.h>
class StdioOutputCallbacks :
    public IDebugOutputCallbacks
{
public:
    // IUnknown.
    STDMETHOD(QueryInterface)(
        THIS_
        _In_ REFIID InterfaceId,
        _Out_ PVOID* Interface
        );
    STDMETHOD_(ULONG, AddRef)(
        THIS
        );
    STDMETHOD_(ULONG, Release)(
        THIS
        );

    // IDebugOutputCallbacks.
    STDMETHOD(Output)(
        THIS_
        _In_ ULONG Mask,
        _In_ PCSTR Text
        );
};

extern StdioOutputCallbacks g_OutputCb;