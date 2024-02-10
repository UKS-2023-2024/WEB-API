﻿using Application.Shared;
using Domain.Tasks;

namespace Application.Labels.Commands.Queries.FindRepositoryDefaultLabels;

public record FindRepositoryDefaultLabelsQuery(Guid RepositoryId): IQuery<List<Label>>;