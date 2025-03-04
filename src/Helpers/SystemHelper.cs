// namespace GrokCLI.Helpers;

// using System;
// using System.Management; // For hardware info
// using System.Net.Http;
// using System.Text;
// using System.Text.Json;

// class SystemHelper
// {
//     static async Task Main(string[] args)
//     {
//         var fingerprint = GetFingerprint();
//         await SendFingerprintToServer(fingerprint);
//     }

//     static object GetFingerprint()
//     {
//         // Example: Collect basic system info
//         string machineId = GetMachineId();
//         string appId = Guid.NewGuid().ToString(); // Unique per run, persist if needed
//         return new { MachineId = machineId, AppId = appId, Timestamp = DateTime.UtcNow };
//     }

//     static string GetMachineId()
//     {
//         try
//         {
//             using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct"))
//             {
//                 foreach (var obj in searcher.Get())
//                 {
//                     return obj["UUID"]?.ToString() ?? "unknown";
//                 }
//             }
//         }
//         catch (Exception) { /* Handle access issues */ }
//         return "fallback-id";
//     }

//     static async Task SendFingerprintToServer(object fingerprint)
//     {
//         using var client = new HttpClient();
//         var json = JsonSerializer.Serialize(fingerprint);
//         var content = new StringContent(json, Encoding.UTF8, "application/json");
//         var response = await client.PostAsync("http://yourserver:3000/auth", content);
//         Console.WriteLine(await response.Content.ReadAsStringAsync());
//     }
// }
// - **MachineId**: Uses WMI to get a hardware identifier (Windows-specific; adjust for other OSes).
// - **AppId**: A unique ID you could persist locally (e.g., in a file or registry) for consistency.
// - **Sending**: Posts the fingerprint to your server via HTTP.
