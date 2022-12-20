using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;
public sealed record MemberCreatedEvent(Guid MemberId) : IDomainEvent;
