using System;
            namespace OmniPulse.WebUI.Controllers;

            public class ReasoningRequest
            {
                public string Intent { get; set; } = string.Empty;
                public string SequenceId { get; set; } = Guid.NewGuid().ToString();
                public dynamic Payload { get; set; }
            }