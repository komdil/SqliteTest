using SqliteTest;
using (SQliteDBContext context = new SQliteDBContext("MyTest.db"))
{
    context.Database.EnsureCreated();
}