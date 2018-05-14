using MSharp;

namespace Apartments
{
    class EnterPage : SubPage<ApartmentPage>
    {
        public EnterPage()
        {
            Layout(Layouts.FrontEnd);

            Add<Modules.ApartmentForm>();
        }
    }
}