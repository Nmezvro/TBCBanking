using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using TBCBanking.Domain.Models.Publics.Requests;

namespace TBCBanking.Infrastructure.Services.Validators
{
    public class TBCBankingRequestValidators
    {

    }

    public class RegisterClientRequestValidator : AbstractValidator<RegisterClientRequest>
    {
        public RegisterClientRequestValidator(IStringLocalizer<RegisterClientRequest> localizer, Domain.Repositories.IClientRepository clientRepository)
        {
            //იჭრება როცა ერთი ლათინურია მეორე ქართული
            RuleFor(x => x.FirstName).Matches("^[a-zA-Z]{2,50}$|^[ა-ჰ]{2,50}$").WithMessage(m => localizer[nameof(m.FirstName)]);
            RuleFor(x => x.LastName).Matches("^[a-zA-Z]{2,50}$|^[ა-ჰ]{2,50}$").WithMessage(m => localizer[nameof(m.LastName)]);
            RuleFor(x => x.Sex).IsInEnum().WithMessage(m => localizer[nameof(m.Sex)]);
            RuleFor(x => x.PersonalNumber).Length(11).WithMessage(m => localizer[nameof(m.PersonalNumber)]);
            RuleFor(x => x.BirthDate).LessThanOrEqualTo(DateTime.Today.AddYears(-18)).WithMessage(m => localizer[nameof(m.BirthDate)]);
            RuleFor(x => x.City).CustomAsync(async (value, context, token) =>
            {
                if (await clientRepository.CityIsNotValid(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
            });
            RuleForEach(x => x.PhoneNumbers).ChildRules(phone =>
            {
                phone.RuleFor(x => x.Type).IsInEnum().WithMessage(m => localizer[nameof(UpdateClientRequest.PhoneNumbers) + nameof(m.Type)]);
                phone.RuleFor(x => x.Phone).Length(4, 50).WithMessage(m => localizer[nameof(UpdateClientRequest.PhoneNumbers) + nameof(m.Phone)]);
            });
            //RuleForEach(x => x.RelatedClients).ChildRules(r =>
            //{
            //    r.RuleFor(x => x.Type).IsInEnum().WithMessage(m => localizer["RelatedClientType"]);
            //    r.RuleFor(x => x.Id).CustomAsync(async (value, context, token) =>
            //    {
            //        if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
            //    });
            //});
        }
    }

    public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientRequestValidator(IStringLocalizer<UpdateClientRequest> localizer, Domain.Repositories.IClientRepository clientRepository)
        {
            //იჭრება როცა ერთი ლათინურია მეორე ქართული
            RuleFor(x => x.Id).NotEmpty().WithMessage(m => localizer[nameof(m.Id)]);
            RuleFor(x => x.Id).CustomAsync(async (value, context, token) =>
            {
                if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
            });
            RuleFor(x => x.FirstName).Matches("^[a-zA-Z]{2,50}$|^[ა-ჰ]{2,50}$").WithMessage(m => localizer[nameof(m.FirstName)]);
            RuleFor(x => x.LastName).Matches("^[a-zA-Z]{2,50}$|^[ა-ჰ]{2,50}$").WithMessage(m => localizer[nameof(m.LastName)]);
            RuleFor(x => x.Sex).IsInEnum().WithMessage(m => localizer[nameof(m.Sex)]);
            RuleFor(x => x.PersonalNumber).Length(11).WithMessage(m => localizer[nameof(m.PersonalNumber)]);
            RuleFor(x => x.BirthDate).LessThanOrEqualTo(DateTime.Today.AddYears(-18)).WithMessage(m => localizer[nameof(m.BirthDate)]);
            RuleFor(x => x.City).CustomAsync(async (value, context, token) =>
            {
                if (await clientRepository.CityIsNotValid(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
            });
            RuleForEach(x => x.PhoneNumbers).ChildRules(phone =>
            {
                phone.RuleFor(x => x.Type).IsInEnum().WithMessage(m => localizer[nameof(UpdateClientRequest.PhoneNumbers) + nameof(m.Type)]);
                phone.RuleFor(x => x.Phone).Length(4, 50).WithMessage(m => localizer[nameof(UpdateClientRequest.PhoneNumbers) + nameof(m.Phone)]);
            });
        }
    }

    public class PutClientPhotoRequestValidator : AbstractValidator<PutClientPhotoRequest>
    {
        public PutClientPhotoRequestValidator(IStringLocalizer<PutClientPhotoRequest> localizer, Domain.Repositories.IClientRepository clientRepository)
        {
            //უნდა შეიცავდეს გაფართოებას
            RuleFor(x => x.PhotoName).Matches("^[\\w\\s-]{1,100}\\.\\w{3,4}$").WithMessage(m => localizer[nameof(m.PhotoName)]);
            RuleFor(x => x.ClientId).NotEmpty().WithMessage(m => localizer[nameof(m.ClientId)]);
            RuleFor(x => x.ClientId).CustomAsync(async (value, context, token) =>
                {
                    if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
                });
        }
    }

    public class AddClientRelativesRequestValidator : AbstractValidator<AddClientRelativesRequest>
    {
        public AddClientRelativesRequestValidator(IStringLocalizer<AddClientRelativesRequest> localizer, Domain.Repositories.IClientRepository clientRepository)
        {
            RuleForEach(x => x.RelatedClients).ChildRules(r =>
            {
                r.RuleFor(x => x.Type).IsInEnum().WithMessage(m => localizer[nameof(AddClientRelativesRequest.RelatedClients) + nameof(m.Type)]);
                r.RuleFor(x => x.Id).CustomAsync(async (value, context, token) =>
                {
                    if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[nameof(AddClientRelativesRequest.RelatedClients) + context.PropertyName]);
                });
            });
            RuleFor(x => x.ClientId).NotEmpty().WithMessage(m => localizer[nameof(m.ClientId)]);
            RuleFor(x => x.ClientId).CustomAsync(async (value, context, token) =>
                {
                    if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
                });
        }
    }

    public class DeleteClientRelativesRequestValidator : AbstractValidator<DeleteClientRelativesRequest>
    {
        public DeleteClientRelativesRequestValidator(IStringLocalizer<DeleteClientRelativesRequest> localizer, Domain.Repositories.IClientRepository clientRepository)
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage(m => localizer[nameof(m.ClientId)]);
            RuleFor(x => x.ClientId).CustomAsync(async (value, context, token) =>
                {
                    if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
                });
            RuleFor(x => x.RelatedClients).NotEmpty().WithMessage(m => localizer[nameof(m.RelatedClients)]);
        }
    }

    public class DeactivateClientRequestValidator : AbstractValidator<DeactivateClientRequest>
    {
        public DeactivateClientRequestValidator(IStringLocalizer<DeactivateClientRequest> localizer, Domain.Repositories.IClientRepository clientRepository)
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage(m => localizer[nameof(m.ClientId)]);
            RuleFor(x => x.ClientId).CustomAsync(async (value, context, token) =>
                {
                    if (await clientRepository.ClientDoesNotExist(value)) context.AddFailure(context.PropertyName, localizer[context.PropertyName]);
                });
        }
    }

    public class ClientSearchRequestValidator : AbstractValidator<ClientSearchRequest>
    {
        public ClientSearchRequestValidator(IStringLocalizer<ClientSearchRequest> localizer)
        {
            RuleFor(x => x.Page).GreaterThan(0).WithMessage(m => localizer[nameof(m.Page)]);
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage(m => localizer[nameof(m.PageSize)]);
        }
    }
}