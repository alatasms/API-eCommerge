using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Common
{
    public enum ResponseMessages
    {
        UserCreated,
        UserUpdated,
        UserLoggedInSuccessfully,
        UserLoggedInFailed,
        UserGoogleLoggedInFailed,
        UserLoggedInWithGoogleSuccessfully,
        UserLoggedInWithFacebookSuccessfully,
        UserFacebookLoggedInFailed,
        PasswordChangedSuccesfully,
        EmailConfirmedSuccessfully,
        EmailShouldBeConfirmed
    }
}
