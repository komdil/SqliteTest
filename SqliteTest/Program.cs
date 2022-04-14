using SqliteTest;
using (SQliteDBContext context = new SQliteDBContext(SubscriberCacheConstants.GetDatabaseName()))
{
    context.Database.EnsureCreated();
    Console.ReadLine();
}