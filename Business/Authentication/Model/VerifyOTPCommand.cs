﻿using Core.Entities.Concrete;
using Core.Utilities.Results;
using MediatR;

namespace Business.Authentication.Model
{
    /// <summary>
    /// It is the command sent to the user for One Time Password control.
    /// </summary>
    public class VerifyOtpCommand : IRequest<IDataResult<PineToken>>
    {
        public AuthenticationProviderType Provider { get; set; }

        /// <summary>
        /// Specifies the subtype so that the same provider user can enter from different systems.
        /// </summary>
        public string ProviderSubType { get; set; }

        public string ExternalUserId { get; set; }
        public int Code { get; set; }
    }
}