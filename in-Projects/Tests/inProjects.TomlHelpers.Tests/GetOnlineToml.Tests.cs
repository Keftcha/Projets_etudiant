using System;
using System.IO;
using NUnit.Framework;

namespace inProjects.TomlHelpers.Tests
{
    [TestFixture]
    public class GetOnlineToml
    {

        [Test]
        public void is_the_downloaded_string_is_correct()
        {
            string offlineToml = File.ReadAllText("./toml_sample_for_tests/ficheprojet_in-projects.toml");
            string onlineToml =  TomlHelpers.GetOnlineToml(GetTomlFromGoogleDrive.GetUrlRessource("https://drive.google.com/open?id=1TSwBj4-pMtqnUCoSJqkZVte0Ah2Z3fxC"));

            Assert.That(onlineToml, Is.EqualTo(offlineToml));
        }

        [Test]
        public void is_the_downloaded_file_a_valid_project()
        {
            ProjectPi onlineProject = TomlHelpers.GetInstanceFromToml<ProjectPi>(TomlHelpers.GetOnlineToml(GetTomlFromGoogleDrive.GetUrlRessource("https://drive.google.com/open?id=1TSwBj4-pMtqnUCoSJqkZVte0Ah2Z3fxC")));

            Assert.That(onlineProject.isValid().Item1, Is.True);
        }

        [Test]
        public void download_then_get_the_toml_as_object()
        {
            string offlineToml = File.ReadAllText("./toml_sample_for_tests/ficheprojet_in-projects.toml");
            string onlineToml =  TomlHelpers.GetOnlineToml(GetTomlFromGoogleDrive.GetUrlRessource("https://drive.google.com/open?id=1TSwBj4-pMtqnUCoSJqkZVte0Ah2Z3fxC"));
            ProjectPi offlineProject = TomlHelpers.GetInstanceFromToml<ProjectPi>(offlineToml);
            ProjectPi onlineProject = TomlHelpers.GetInstanceFromToml<ProjectPi>(onlineToml);

            Assert.That(onlineProject, Is.EqualTo(onlineProject));
        }

        [TestCase("http://this_site_doesn't_exist.org/poulet")]
        [TestCase("https://drive.google.com/open?id=i_hope_this_id_doesnt_exist")]
        [TestCase("zisa.fr")]
        public void errors_shound_occur_when_the_given_url_isnt_good(string url)
        {
            Assert.Catch(() => TomlHelpers.GetOnlineToml(url));
        }
    }
}
