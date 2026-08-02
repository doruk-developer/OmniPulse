using System;
            using MediatR;

            namespace OmniPulse.Entities.Events;

            public record ClusterStateSynchronizedEvent : INotification { public dynamic State { get; init; } public DateTime Timestamp { get; init; } }
            public record GenerateForensicV5Command : IRequest<object> { public string TargetNode { get; init; } }
            public record ProcessCognitiveChainQuery : IRequest<object> { public dynamic Payload { get; init; } }