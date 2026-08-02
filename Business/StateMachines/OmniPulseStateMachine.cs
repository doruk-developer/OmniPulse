using MassTransit;
using System;
namespace OmniPulse.Business.StateMachines;

public record NodeInitEvent(Guid CorrelationId);

public class OmniPulseSagaState : MassTransit.SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public DateTime UpdatedAt { get; set; }

}

public class OmniPulseStateMachine : MassTransitStateMachine<OmniPulseSagaState>
{
    public OmniPulseStateMachine()
    {
        InstanceState(x => x.CurrentState);

        // [HIGH_SAGA_TRANSITION_LOGIC_START]

        Initially(
            When(NodeInitialized)
                .Then(context => context.Saga.UpdatedAt = DateTime.UtcNow)
                .TransitionTo(Processing)
        );

        // [HIGH_SAGA_TRANSITION_LOGIC_END]
    }

    public State Processing { get; private set; }
    public Event<NodeInitEvent> NodeInitialized { get; private set; }
}