using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibly.EmailService.Api.Dtos.Enums;
using Codibly.EmailService.Api.Dtos.Models;
using Codibly.EmailService.Api.Models.Models;
using Codibly.EmailService.Api.Models.Models.Enums;
using Codibly.EmailService.Api.Services.Exceptions;
using Codibly.EmailService.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EmailModel = Codibly.EmailService.Api.Models.Models.Email;

namespace Codibly.EmailService.Api.Services.Tests.Services
{
    public class EmailServiceTests : ServiceTestBase<DbSet<EmailModel>, EmailModel>
    {
        #region Private classes

        private class EmailServiceTestData
        {
            #region Properties

            public static IEnumerable<object[]> CreateEmailData =>
                new List<object[]>
                {
                    new object[]
                    {
                        CreateEmailDataForPost(),
                        3,
                        "user@email.com"
                    }
                };

            public static IEnumerable<object[]> CreateEmailValidationExceptionData =>
                new List<object[]>
                {
                    new object[]
                    {
                        null,
                        "Email data is empty"
                    },
                    new object[]
                    {
                        new EmailCreateableDto
                        {
                            Recipients = null,
                        },
                        "Invalid number of recipients. Min allowed recipients is: 1"
                    },
                    new object[]
                    {
                        new EmailCreateableDto
                        {
                            Recipients = new List<string>(),
                        },
                        "Invalid number of recipients. Min allowed recipients is: 1"
                    }
                };

            public static IEnumerable<object[]> UpdateEmailData =>
                new List<object[]>
                {
                    new object[]
                    {
                        1,
                        CreateEmailDataForUpdate(),
                        "Test email changed",
                        "Content of test email changed",
                        new List<string>
                        {
                            "first_recipient@email.com",
                            "second_recipient@email.com"
                        }
                    }
                };

            public static IEnumerable<object[]> UpdateEmailValidationExceptionData =>
                new List<object[]>
                {
                    new object[]
                    {
                        1,
                        null,
                        "Email data is empty"
                    },
                    new object[]
                    {
                        2,
                        CreateEmailDataForUpdate(),
                        "Invalid email id value"
                    },
                    new object[]
                    {
                        1,
                        new EmailDto
                        {
                            Id = 1,
                            Recipients = new List<string>()
                        },
                        "Invalid number of recipients. Min allowed recipients is: 1"
                    },
                    new object[]
                    {
                        1,
                        new EmailDto
                        {
                            Id = 1,
                            Recipients = null
                        },
                        "Invalid number of recipients. Min allowed recipients is: 1"
                    }
                };

            public static IEnumerable<object[]> UpdateEmailNotFoundExceptionData =>
                new List<object[]>
                {
                    new object[]
                    {
                        3,
                        new EmailDto
                        {
                            Id = 3,
                            Recipients = new List<string>
                            {
                                "first_recipient@email.com",
                                "second_recipient@email.com"
                            }
                        },
                        "Email with id: 3 was not found"
                    }
                };

            #endregion
        }

        #endregion

        #region Public methods

        [Fact]
        public async Task VerifyGetAllEmails()
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            IEnumerable<EmailHeaderDto> result = await testCandidate.GetAllEmails();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task VerifyGetAllPendingEmails()
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            ICollection<EmailModel> result = await testCandidate.GetAllPendingEmails();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.All(result, email => Assert.Equal(EmailStateEnum.Pending, email.State));
        }

        [Fact]
        public async Task VerifyGetEmail()
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            EmailDto result = await testCandidate.GetEmail(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Theory]
        [InlineData(3, "Email with id: 3 was not found")]
        public async Task VerifyGetEmailThrowsNotFoundException(int id, string expectedExceptionMessage)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => testCandidate.GetEmail(id));

            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [InlineData(1, EmailStateEnumDto.Pending)]
        [InlineData(2, EmailStateEnumDto.Send)]
        public async Task VerifyGetEmailState(int id, EmailStateEnumDto expected)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            EmailStateEnumDto result = await testCandidate.GetEmailState(id);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(3, "Email with id: 3 was not found")]
        public async Task VerifyGetEmailStateThrowsNotFoundException(int id, string expectedExceptionMessage)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => testCandidate.GetEmailState(id));

            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(EmailServiceTestData.CreateEmailData), MemberType = typeof(EmailServiceTestData))]
        public async Task VerifyPostEmail(EmailCreateableDto data, int expectedId, string expectedCreatedBy)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            EmailDto result = await testCandidate.PostEmail(data);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.Equal(expectedCreatedBy, result.CreatedBy);
            Assert.Equal(1, DbContext.Recipients.Count(r => r.EmailId == result.Id));
        }

        [Theory]
        [MemberData(nameof(EmailServiceTestData.CreateEmailValidationExceptionData), MemberType = typeof(EmailServiceTestData))]
        public async Task VerifyPostEmailThrowsValidationExceptions(EmailCreateableDto data, string expectedExceptionMessage)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            var exception = await Assert.ThrowsAsync<ValidationException>(() => testCandidate.PostEmail(data));

            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public async Task VerifyUpdateEmailState()
        {
            IEmailService testCandidate = CreateDefaultEmailService();
            var utcNow = DateTimeOffset.UtcNow;

            await testCandidate.UpdateEmailState(1, utcNow);
            EmailDto result = await testCandidate.GetEmail(1);

            Assert.Equal(EmailStateEnumDto.Send, result.State);
            Assert.Equal(utcNow, result.SendOn);
        }

        [Theory]
        [InlineData(3, "Email with id: 3 was not found")]
        public async Task VerifyUpdateEmailStateThrowsNotFoundException(int id, string expectedExceptionMessage)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => testCandidate.UpdateEmailState(id, DateTimeOffset.UtcNow));

            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(EmailServiceTestData.UpdateEmailData), MemberType = typeof(EmailServiceTestData))]
        public async Task VerifyUpdateEmail(int id, EmailDto data, string expectedSubject, string expectedContent, List<string> expectedRecipients)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            EmailDto result = await testCandidate.UpdateEmail(id, data);

            Assert.Equal(expectedSubject, result.Subject);
            Assert.Equal(expectedContent, result.Content);
            Assert.Equal(expectedRecipients.Count, result.Recipients.Count);
            Assert.All(result.Recipients, s => Assert.Contains(s, expectedRecipients));
        }

        [Theory]
        [MemberData(nameof(EmailServiceTestData.UpdateEmailValidationExceptionData), MemberType = typeof(EmailServiceTestData))]
        public async Task VerifyUpdateEmailThrowsValidationExceptions(int id, EmailDto data, string expectedExceptionMessage)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            var exception = await Assert.ThrowsAsync<ValidationException>(() => testCandidate.UpdateEmail(id, data));

            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(EmailServiceTestData.UpdateEmailNotFoundExceptionData), MemberType = typeof(EmailServiceTestData))]
        public async Task VerifyUpdateEmailThrowsNotFoundException(int id, EmailDto data, string expectedExceptionMessage)
        {
            IEmailService testCandidate = CreateDefaultEmailService();

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => testCandidate.UpdateEmail(id, data));

            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        #endregion

        #region Private methods

        private IEmailService CreateDefaultEmailService()
        {
            var emails = new List<EmailModel>
            {
                new EmailModel
                {
                    Id = 1,
                    Subject = "Test email",
                    Sender = "user@email.com",
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Id = 1,
                            EmailAddress = "recipient@email.com",
                            EmailId = 1
                        }
                    },
                    Content = "Content of test email",
                    State = EmailStateEnum.Pending,
                    CreatedBy = "user@email.com",
                    CreatedOn = DateTimeOffset.UtcNow,
                },
                new EmailModel
                {
                    Id = 2,
                    Subject = "Test email",
                    Sender = "user@email.com",
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Id = 2,
                            EmailAddress = "recipient@email.com",
                            EmailId = 2
                        }
                    },
                    Content = "Content of test email",
                    State = EmailStateEnum.Send,
                    CreatedBy = "user@email.com",
                    CreatedOn = DateTimeOffset.UtcNow,
                    SendOn = DateTimeOffset.UtcNow
                }
            };

            FillDatabase(DbContext, DbContext.Emails, emails);

            return new Api.Services.Services.EmailService(DbContext, CreateDefaultMapper());
        }

        private static EmailCreateableDto CreateEmailDataForPost()
        {
            return new EmailCreateableDto
            {
                Subject = "Test email",
                Sender = "user@email.com",
                Recipients = new List<string>
                {
                    "recipient@email.com"
                },
                Content = "Content of test email",
            };
        }

        private static EmailDto CreateEmailDataForUpdate()
        {
            return new EmailDto
            {
                Id = 1,
                Subject = "Test email changed",
                Sender = "user@email.com",
                Recipients = new List<string>
                {
                    "first_recipient@email.com",
                    "second_recipient@email.com"
                },
                Content = "Content of test email changed",
                State = EmailStateEnumDto.Pending,
                CreatedBy = "user@email.com",
                CreatedOn = DateTimeOffset.UtcNow,
            };
        }

        #endregion
    }
}
