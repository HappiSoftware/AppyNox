using AppyNox.Services.Sso.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Sso.Infrastructure.Localization;
using AppyNox.Services.Sso.SharedEvents.Events;
using AppyNox.Services.License.SharedEvents.Events;
using MassTransit;

namespace AppyNox.Services.Sso.Infrastructure.MassTransit.Sagas
{
    public class UserCreationSaga : MassTransitStateMachine<UserCreationSagaState>
    {
        #region [ Public Constructors ]

        public UserCreationSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => StartUserCreationMessage, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => LicenseValidatedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => ApplicationUserCreatedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(StartUserCreationMessage)
                    .Then(context =>
                    {
                        context.Saga.CorrelationId = context.CorrelationId
                            ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.UserCreationSagaCorrelationIdError, (int)NoxSsoInfrastructureExceptionCode.UserCreationSagaCorrelationId);

                        context.Saga.LicenseKey = context.Message.LicenseKey;
                        context.Saga.UserName = context.Message.UserName;
                        context.Saga.Email = context.Message.Email;
                        context.Saga.Password = context.Message.Password;
                        context.Saga.ConfirmPassword = context.Message.ConfirmPassword;
                    })
                    .Send(new Uri("queue:validate-license"), context => new ValidateLicenseMessage
                    (
                        context.Saga.CorrelationId,
                        context.Saga.LicenseKey
                    ))
                    .TransitionTo(ValidatingLicense));

            During(ValidatingLicense,
                When(LicenseValidatedEvent)
                    .If(context => context.Message.IsValid, x => x
                        .Then(context =>
                        {
                            context.Saga.LicenseId = context.Message.LicenseId
                            ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.UserCreationSagaCorrelationIdError, (int)NoxSsoInfrastructureExceptionCode.UserCreationSagaLicenseIdNull);
                        })
                        .Send(new Uri("queue:create-user"), context => new CreateApplicationUserMessage
                            (
                                context.Saga.CorrelationId,
                                context.Saga.UserName,
                                context.Saga.Email,
                                context.Saga.Password,
                                context.Saga.ConfirmPassword,
                                context.Message.CompanyId
                                    ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.UserCreationSagaCompanyIdError, (int)NoxSsoInfrastructureExceptionCode.UserCreationSagaLicenseIdNull)
                            ))
                        .TransitionTo(CreatingUser)
                    ));

            During(CreatingUser,
                When(ApplicationUserCreatedEvent)
                    .Then(context =>
                    {
                        context.Saga.UserId = context.Message.UserId;
                    })
                    .Send(new Uri("queue:assign-license-to-user"), context => new AssignLicenseToUserMessage
                    (
                        context.Saga.CorrelationId,
                        context.Saga.UserId,
                        context.Saga.LicenseId
                    )),
                When(RevertApplicationUserCreationEvent)
                    .Then(context =>
                    {
                        context.Send(new Uri("queue:delete-user"), new DeleteApplicationUserMessage
                            (
                                context.Saga.CorrelationId,
                                context.Message.UserId
                            ));
                    })
                    .Finalize());
        }

        #endregion

        #region [ Properties ]

        // Define states
        public State ValidatingLicense { get; private set; } = default!;

        public State CreatingUser { get; private set; } = default!;

        // Define events
        public Event<StartUserCreationMessage> StartUserCreationMessage { get; private set; } = default!;

        public Event<LicenseValidatedEvent> LicenseValidatedEvent { get; private set; } = default!;

        public Event<ApplicationUserCreatedEvent> ApplicationUserCreatedEvent { get; private set; } = default!;

        public Event<RevertApplicationUserCreationEvent> RevertApplicationUserCreationEvent { get; private set; } = default!;

        #endregion
    }

    public class UserCreationSagaState : SagaStateMachineInstance
    {
        #region [ Properties ]

        public Guid CorrelationId { get; set; }

        public string? CurrentState { get; set; }

        public string LicenseKey { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid LicenseId { get; set; }

        #endregion
    }
}