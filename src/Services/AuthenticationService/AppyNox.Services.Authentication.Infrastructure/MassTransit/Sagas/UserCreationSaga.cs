using AppyNox.Services.Authentication.SharedEvents.Events;
using AppyNox.Services.License.SharedEvents.Events;
using MassTransit;

namespace AppyNox.Services.Authentication.Infrastructure.MassTransit.Sagas
{
    public class UserCreationSaga : MassTransitStateMachine<UserCreationSagaState>
    {
        #region Public Constructors

        public UserCreationSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => StartUserCreation, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => LicenseValidationCompleted, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => UserCreationCompleted, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(StartUserCreation)
                    .Then(context =>
                    {
                        context.Saga.LicenseKey = context.Message.LicenseKey;
                    })
                    .Send(new Uri("queue:validate-license"), context => new LicenseValidationRequested
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        LicenseKey = context.Saga.LicenseKey
                    })
                    .TransitionTo(ValidatingLicense));

            During(ValidatingLicense,
                When(LicenseValidationCompleted)
                    .Send(new Uri("queue:create-user"), context => new ApplicationUserCreateRequested
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        UserName = context.Saga.UserName,
                        Password = context.Saga.Password,
                        ConfirmPassword = context.Saga.ConfirmPassword,
                        Email = context.Saga.Email
                    })
                    .TransitionTo(CreatingUser));

            During(CreatingUser,
                When(UserCreationCompleted)
                    .Then(context =>
                    {
                        context.Saga.UserId = context.Message.UserId;
                    })
                    .Send(new Uri("queue:assign-license-to-user"), context => new AssignLicenseToUser
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        UserId = context.Saga.UserId,
                        LicenseKey = context.Saga.LicenseKey
                    })
                    .Finalize());
        }

        #endregion

        #region Properties

        // Define states
        public State ValidatingLicense { get; private set; } = default!;

        public State CreatingUser { get; private set; } = default!;

        // Define events
        public Event<StartUserCreation> StartUserCreation { get; private set; } = default!;

        public Event<LicenseValidationCompleted> LicenseValidationCompleted { get; private set; } = default!;

        public Event<ApplicationUserCreateCompleted> UserCreationCompleted { get; private set; } = default!;

        #endregion
    }

    public class UserCreationSagaState : SagaStateMachineInstance
    {
        #region Properties

        public Guid CorrelationId { get; set; }

        public string? CurrentState { get; set; }

        public string LicenseKey { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        #endregion
    }
}