using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;
using Process = System.Diagnostics.Process;
using System.IO.Compression;



namespace AutomatedDataCollectionApi.Services
{
    public class DataProcessServices : IDataProcessService
    {
        private readonly IConfiguration _configuration;

        public DataProcessServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private const string EndpointsFile = "Services/apis_parsed.txt";

        public List<string> GetConfigFileEndPoints()
        {
            // Read endpoints file into list
            var endpoints = File.ReadAllLines(EndpointsFile)
                               .ToList();

            return endpoints;
        }

        public async Task<string> AddConfigFileEndPoints(List<string> newEndpoints)
        {
            // Get existing endpoints
            var currentEndpoints = GetConfigFileEndPoints();

            // Check if any of the new endpoints already exist
            var duplicateEndpoints = newEndpoints.Intersect(currentEndpoints).ToList();
            if (duplicateEndpoints.Count > 0)
            {
                string duplicateEndpointsMessage = string.Join(", ", duplicateEndpoints);
                return $"Endpoints already exist: {duplicateEndpointsMessage}";
            }

            // Append new endpoints
            currentEndpoints.AddRange(newEndpoints);

            // Write combined list back to file
            File.WriteAllLines(EndpointsFile, currentEndpoints);

            await Task.Delay(100);
            return "Success";
        }



        public async Task<string> EditConfigFileEndPoints(List<string> newEndpoints)
        {
            // Read existing endpoints
            var existingEndpoints = GetConfigFileEndPoints();

            // Modify existing endpoints based on your requirement
            // For example, you can use a loop or LINQ to replace/edit specific endpoints

            // Replace the specific endpoints with new values
            foreach (var endpoint in newEndpoints)
            {
                var index = existingEndpoints.IndexOf(endpoint);
                if (index != -1)
                {
                    existingEndpoints[index] = "new value"; // Update with the new value
                }
            }

            // Write back the modified endpoints
            File.WriteAllLines(EndpointsFile, existingEndpoints);

            await Task.Delay(100); // Simulate async operation

            return "Success";
        }

        public async Task<string> DeleteConfigFileEndPoints(List<string> endpointsToDelete)
        {
            // Read existing endpoints
            var existingEndpoints = GetConfigFileEndPoints();

            // Remove the specified endpoints
            existingEndpoints.RemoveAll(endpoint => endpointsToDelete.Contains(endpoint));

            // Write back the modified endpoints
            File.WriteAllLines(EndpointsFile, existingEndpoints);

            await Task.Delay(100); // Simulate async operation

            return "Success";
        }

        public async Task<string> RunPythonScript()
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "C:/Python311/python.exe"; // Assuming 'python' is in your PATH environment variable

                string scriptPath = "./Automation/Finals/data_processing.py";
                start.Arguments = scriptPath;
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;

                using (Process? process = Process.Start(start))
                {
                    if (process == null)
                    {
                        throw new Exception("Failed to start Python process.");
                    }

                    using (StreamReader reader = process.StandardOutput)
                    using (StreamReader errorReader = process.StandardError)
                    {
                        string result = await reader.ReadToEndAsync();
                        string error = await errorReader.ReadToEndAsync();
                        Console.WriteLine("Python Output: " + result);
                        Console.WriteLine("Python Error: " + error);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public void CreateZipArchive(string sourceFolderPath, string zipPath)
        {
            // Make sure the source folder exists
            if (!Directory.Exists(sourceFolderPath))
            {
                throw new DirectoryNotFoundException("Source folder not found.");
            }

            // Create the zip archive
            ZipFile.CreateFromDirectory(sourceFolderPath, zipPath);
        }




    }
}

