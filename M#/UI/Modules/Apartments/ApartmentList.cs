using MSharp;

namespace Modules
{
    public class ApartmentList : ListModule<Domain.Apartment>
    {
        public ApartmentList()
        {
            HeaderText("Asset Typeses");

            
            Column(x => x.Name);
            Column(x => x.BuildTime);

            ButtonColumn("Edit").Icon(FA.Edit)
                .OnClick(x => x.Go<Apartments.EnterPage>()
                .Send("item", "item.ID")
                .SendReturnUrl());

            ButtonColumn("Delete").Icon(FA.Remove)
                .OnClick(x =>
                {
                    x.DeleteItem();
                    x.Reload();
                });

            Button("New Asset Types").Icon(FA.Plus)
                .OnClick(x => x.Go<Apartments.EnterPage>()
                .SendReturnUrl());
        }
    }
}