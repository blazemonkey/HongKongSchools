using HongKongSchools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.Interfaces
{
    public interface IMainPageViewModel
    {
        void ExecuteTapSettingsCommand();
        void ExecuteTapSchoolCommand(School school);
    }
}
