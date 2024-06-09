using Intrinsic.Sockets.Server.DTOs;

namespace Intrinsic.Sockets.Client;

internal class RandomDataGenerator
{
    private static readonly string[] Names = [
        "Michal",
        "Marek", 
        "Mirek", 
        "Mona", 
        "Minnie"];

    private static string[] Emails => [
        $"Michal-{Guid.NewGuid()}@gmail.com", 
        "Marek@gmail.com",
        $"Mirek-{Guid.NewGuid()}@gmail.com", 
        "Mona@gmail.com",
        $"Minnie-{Guid.NewGuid()}@gmail.com"];

    public static ServerRequestDto GenerateServerRequestDto()
    {
        return new ServerRequestDto(
            GetRandomItem(Names),
            Random.Shared.Next(1, 100),
            GetRandomItem(Emails));
    }

    private static T GetRandomItem<T>(T[] array)
    {
        return array[Random.Shared.NextInt64(array.Length)];
    }
}
