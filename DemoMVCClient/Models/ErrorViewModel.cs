using System;
using IdentityServer4.Models;

namespace DemoMVCClient.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public ErrorMessage Error { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}