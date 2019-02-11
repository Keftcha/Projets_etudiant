using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViWork.DAL.Test
{
    [TestFixture]
    public class UsersGateAwayTest
    {
        readonly Random _random = new Random();

        [Test]
        public async Task can_create_find_update_and_delete_user()
        {
            UserGateway sut = new UserGateway(TestHelpers.ConnectionString);
            
            string email = "user{0}@test.com";
            string firstName ="test";
            string lastName = "test";
            byte[] password = Guid.NewGuid().ToByteArray();
        

            Result<int> result = await sut.CreatePasswordUser(email, password,firstName,lastName);
            UserData user = await sut.FindById(result.Content);

            {
                Assert.That(user.Email, Is.EqualTo(email));
                Assert.That(user.Password, Is.EqualTo(password));
            }

            {
                UserData u = await sut.FindById(user.UserId);
                Assert.That(u.Email, Is.EqualTo(email));
                Assert.That(u.Password, Is.EqualTo(password));
            }

            {
                email = string.Format("user{0}@test.com", Guid.NewGuid());
                await sut.UpdateEmail(user.UserId, email);
            }

            {
                UserData u = await sut.FindById(user.UserId);
                Assert.That(u.Email, Is.EqualTo(email));
                Assert.That(u.Password, Is.EqualTo(password));
            }

            {
                await sut.Delete(user.UserId);
                Assert.That(await sut.FindById(user.UserId), Is.Null);
            }
        }


        [Test]

        public async Task can_create_github_user()
        {
            UserGateway sut = new UserGateway(TestHelpers.ConnectionString);
            string email = "email@gmail.com";
            int githubId = 124412415;
            string accessToken = "34fe2-43g3g-34g33-3g43";

            await sut.CreateOrUpdateGithubUser(email, githubId, accessToken);
            UserData user = await sut.FindByEmail(email);

            Assert.That(user.GithubAccessToken, Is.EqualTo(accessToken));

            accessToken = "34rg2-4rg3g-34g3w-3g43";
            await sut.CreateOrUpdateGithubUser(user.Email, user.GithubId, accessToken);

            user = await sut.FindById(user.UserId);
            Assert.That(user.GithubAccessToken, Is.EqualTo(accessToken));

            await sut.Delete(user.UserId);
        }

    }

}
