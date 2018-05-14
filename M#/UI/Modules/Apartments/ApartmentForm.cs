using MSharp;

namespace Modules
{
    public class ApartmentForm : FormModule<Domain.Apartment>
    {
        public ApartmentForm()
        {
            HeaderText("Apartment details");

            Field(x => x.Name);

            Field(x => x.BuildTime);

            Button("Cancel").OnClick(x => x.ReturnToPreviousPage());

            Button("Save").IsDefault().Icon(FA.Check)
            .OnClick(x =>
            {
                x.SaveInDatabase();
                x.GentleMessage("Saved successfully.");
                x.ReturnToPreviousPage();
            });
        }
    }
}