﻿using AutoWrapper.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppyNox.Services.Base.IntegrationTests.Helpers
{
    public static class ApiResponseHelpers
    {
        #region [ Public Methods ]

        public static void ValidateOk(this ApiResponse apiResponse)
        {
            //Assert.Null(apiResponse.IsError);
            //Assert.Null(apiResponse.ResponseException);
            Assert.NotNull(apiResponse.Result);
            Assert.Equal((int)HttpStatusCode.OK, apiResponse.StatusCode);

            //Assert.True(!string.IsNullOrEmpty(apiResponse.Message) && apiResponse.Message.Contains("Successful", StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}