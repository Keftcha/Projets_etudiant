using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Nett;
using inProjects.Data;
using CK.SqlServer.Setup;
using System.Threading.Tasks;
using System.Net.Mail;
using inProjects.Data.Queries;
using CK.Core;
using CK.SqlServer;
using System.Collections.Generic;

namespace inProjects.TomlHelpers
{
    public static class TomlHelpers
    {
        public static T GetInstanceFromToml<T>(string toml)
        {
            return Toml.ReadString<T>(toml);
        }

        public static string GetOnlineToml(string url)
        {
            string str = new WebClient().DownloadString(url);
            return str;
        }

        public static async Task<(bool, string)> RegisterProject(
            string url,
            int projectNumber,
            ProjectStudentTable projectTable,
            int userId,
            SqlDefaultDatabase db,
            CustomGroupTable groupTable,
            IStObjMap stObjMap,
            ProjectUrlTable projectUrlTable
        )
        // given url, we register the project if he can be downloaded and parsed
        {
            string tomlString;
            Project toml;
            Type projectType;

            try    // get the type of the project
            {
                projectType = new Type[]{typeof(ProjectPi), typeof(ProjectPfh)}[projectNumber];
            }
            catch
            {
                return (false, "Le numéro pour le type de projet n'est pas bon.");
            }

            try    // to get the ressource
            {
                tomlString = GetOnlineToml(GetTomlFromGoogleDrive.GetUrlRessource(url));
            }
            catch
            {
                return (false, "La ressource n'est pas trouvée, peut-être que le liens n'est pas le bon.");
            }

            try    // parse the toml
            {
                var method = typeof(TomlHelpers).GetMethod("GetInstanceFromToml");
                method = method.MakeGenericMethod(projectType);
                toml = (Project)method.Invoke(null, new object[] { tomlString }); 
            }
            catch(Exception e)
            {
                List<string> foo = e.ToString().Split("-->")[1].Split(":").Skip(1).Take(2).ToList();
                foo[1] = foo[1].Split(".")[0];

                return (false, "Impossible de parser le fichier toml, est-ce que le fichier est correctement formaté ?\n" + String.Join(": ", foo));
            }

            (bool isValid, string message) = toml.isValid();
            if(!isValid)    // check if we got fild we want
            {
                return (false, message);    // "There is missing or bad field in the toml file");
            }

            toml.logo.url = GetTomlFromGoogleDrive.GetUrlRessource(toml.logo.url);
            

            if(toml.team.leader!= "None" && !await toml.team.isMailExisting(toml.team.leader, stObjMap)) return (false, "L'e-mail du chef de projet est invalide.");
            foreach(string mail in toml.team.members ) { if( !await toml.team.isMailExisting(mail, stObjMap)) return (false, "L'un des e-mails des membres est invalide."); };
            try    // register the project in the bdd
            {
                await RegisterProjectInBDD.SaveProject(projectType, toml, userId, db, projectTable, groupTable, projectUrlTable);
            }
            catch(Exception e) 
            {
                return (false, "Impossible de sauvegarder le projet dans la BDD.");
            }

            return (true, "Le projet a été sauvegarde avec succès.");
        }
    }

    // class project
    public class Project
    {
        public Name name {get; set;}
        public Semester semester {get; set;}
        public Logo logo {get; set;}
        public Slogan slogan {get; set;}
        public Pitch pitch {get; set;}
        public Team team {get; set;}
        public OthersDocuments othersDocuments {get; set;}

        public (bool, string) isValid()
        {
            // properties that are optional, they can be null
            string[] optionalProperties = new string[]{"othersDocuments"};

            foreach (PropertyInfo propertie in this.GetType().GetProperties())
            {
                object propertieValue = propertie.GetValue(this, null);

                if (propertieValue is null && !optionalProperties.Contains(propertie.Name)) return (false, "La propriété \"" + propertie.Name.ToString() + "\" est absente.");
                if (propertieValue is IProjectField && !(propertieValue as IProjectField).isValid()) return (false, "La propriété \"" + propertie.Name.ToString() + "\" est invalide.");
            }
            return (true, "All good");
        }
    }

    public class ProjectPi : Project
    {
        public Technologies technologies {get; set;}
        public Git git {get; set;}
    }

    public class ProjectPfh : Project
    {
        public Background background {get; set;}
    }

    // class of fields in project
    interface IProjectField
    {
        bool isValid();
    }

    public class Name: IProjectField
    {
        public string project_name {get; set;}
        public bool isValid(){return this.project_name != null && this.project_name != "";} 

    }

    public class Semester: IProjectField
    {
        public int[] semester {get; set;}
        public string sector {get; set;}

        public bool isValid()
        {
            int[] avariable_semester = new int[]{1, 2, 3, 4, 5};

            if (semester == null || semester.Length == 0 || !new string[]{"IL", "SR", "IL - SR", "None"}.Contains(this.sector)) return false;

            foreach (int elem in this.semester) {
                if (!avariable_semester.Contains(elem)) return false;
            }

            return true;
        }
    }

    public class Logo: IProjectField
    {
        public string url {get; set;}
        public bool isValid(){return this.url != null && this.url != "";} 
    }

    public class Slogan: IProjectField
    {
        public string slogan {get; set;}
        public bool isValid(){return this.slogan != null && this.slogan != "";} 
    }

    public class Pitch: IProjectField
    {
        public string pitch {get; set;}

        public bool isValid(){return this.pitch != null && this.pitch != "";} 
    }

    public class Team: IProjectField
    {
        public string leader {get; set;}
        public string[] members {get; set;}

        public bool isValid()
        {
            // the leader isn't renseigned
            if (leader == null || leader == "" || members == null) {return false;}
            if (leader == "None")    // there is no leader
            {
                // the list member must have people in it
                if (this.members == null || this.members.Length == 0){return false;}
                // people in the list can't be null or empty string
                foreach (string member in members){if (member == null || member == "" || !Team.isValidEmail(member)) return false;}
                return true;
            }
            else    // there is a leader
            {
                if (!Team.isValidEmail(leader)) return false;
                //check the leader mail if there is a leader
                foreach (string member in members){if (member == null || member == "" || !Team.isValidEmail(member)) return false;}
                return true;
            }
                
        }

        private static bool isValidEmail(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> isMailExisting(string email, IStObjMap stObjMap )
        {
            try
            {
                SqlDefaultDatabase db = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
                using( var ctx = new SqlStandardCallContext() )
                {
                    UserQueries groupQueries = new UserQueries( ctx, db );
                    int exists = await groupQueries.CheckEmail( email );
                    if( exists != 0 ) return true;
                    else return false;
                    
                }
            }
            catch
            {
                return false;
            }
        }
    }

    public class Git: IProjectField
    {
        public string url {get; set;}

        public bool isValid()
        {
            if( this.url == null || this.url == "") return false;
            return true;
        }
    }

    public class OthersDocuments: IProjectField
    {
        public string[] urls {get; set;}

        public bool isValid()
        {
            if (this.urls == null || this.urls.Length <= 0) return false;
            foreach (string url in this.urls){if (url is null || url is "") return false;}

            return true;
        }
    }

    public class Technologies: IProjectField
    {
        public string[] technologies {get; set;}

        public bool isValid()
        {
            // the list have elements in it
            if (this.technologies == null || this.technologies.Length <= 0){return false;}
            // no empty element
            foreach (string techno in technologies){if (techno == null || techno == "") return false;}

            return true;
        }
    }

    public class Background: IProjectField
    {
        public string image {get; set;}

        public bool isValid() {return this.image != null && this.image != "";}
    }
}
