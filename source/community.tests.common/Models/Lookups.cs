namespace community.tests.common.Models;

public static class PhoneContactMethods
{
    public static readonly Guid MainPhone = Guid.Parse("01950294-15e0-7bd7-b232-4cfe690f1bb5");
    public static readonly Guid HomePhone = Guid.Parse("01950294-15e2-70e1-b1b1-172ea8b81d87");
    public static readonly Guid MobilePhone = Guid.Parse("01950294-15e2-7287-a4c7-c17d6c38d4d8");
    public static readonly Guid WorkPhone = Guid.Parse("01950294-15e2-732f-ac22-1ff1ab6ae67c");
    public static readonly Guid EmergencyContact = Guid.Parse("01950294-15e2-7374-969a-dd871e567604");
}

public static class EmailContactMethods
{
    public static readonly Guid PersonalEmail = Guid.Parse("01950294-15e2-73a1-8772-dc729ab46f37");
    public static readonly Guid WorkEmail = Guid.Parse("01966570-ab82-7b1a-ba12-77e18833cecf");
}

public static class AddressTypes
{
    public static readonly Guid Primary = Guid.Parse("01950294-15c6-77eb-92d2-769bb5c72fcf");
    public static readonly Guid Home = Guid.Parse("01950294-15c7-732b-ae70-ca05fe33a8f9");
    public static readonly Guid Business = Guid.Parse("01950294-15c7-73a9-bbd1-05d30f8884b8");
    public static readonly Guid Billing = Guid.Parse("01950294-15c7-73db-9c5e-aa1542cd2f55");
    public static readonly Guid Mailing = Guid.Parse("01950294-15c7-7400-86f3-56db1d6f026b");
    public static readonly Guid Contact = Guid.Parse("01950294-15c7-7424-9746-39ed45ba8b60");
    public static readonly Guid AccountsReceivable = Guid.Parse("01950294-15c7-7449-ab20-0a33e305efda");
    public static readonly Guid Recipient = Guid.Parse("0196077a-7425-7533-8c0c-e4ef604d8324");
}

public static class UserTypes
{
    public static readonly Guid SiteAdministrator = Guid.Parse("0194e2a0-1dd9-7d58-bdb6-f8eae6732cc2");
    public static readonly Guid SupportAdministrator = Guid.Parse("0194e2a0-1ddb-7ced-92ec-6a2ee36ed7db");
    public static readonly Guid CommunityAdministrator = Guid.Parse("0194e2a0-1ddb-7d70-aa17-4087a157d6a2");
    public static readonly Guid CommunityMember = Guid.Parse("0194e2a0-1ddb-7da9-bf6e-c9fea6787578");
}