using AvaloniaEdit.Snippets;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using AvalonStudio.Projects;
using System.Composition;

namespace AvalonStudio.Controls.Editor.Snippets
{
    [Export(typeof(SnippetManager))]
    [Shared]
    public class SnippetManager : IActivatableExtension
    {
        public SnippetManager ()
        {
            var snippetFolders = Directory.EnumerateDirectories(Platform.SnippetsFolder).Concat(Directory.EnumerateDirectories(Platform.InBuiltSnippetsFolder));

            foreach (var folder in snippetFolders)
            {
                foreach (var file in Directory.EnumerateFiles(folder))
                {
                    try
                    {
                        var snippet = SerializedObject.Deserialize<CodeSnippet>(file);
                        AddSnippet(_snippets, Path.GetFileName(folder), snippet);
                    }
                    catch (Exception e)
                    {
                        IoC.Get<IConsole>().WriteLine($"Error parsing snippet: {file}, {e.Message}");
                    }
                }
            }
        }

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
   
        }

        public void InitialiseSnippetsForSolution(ISolution solution)
        {
            if (!_solutionSnippets.ContainsKey(solution))
            {
                _solutionSnippets[solution] = new Dictionary<string, IDictionary<string, CodeSnippet>>();

                _solutionSnippets[solution].Clear();

                var snippetsDir = Platform.GetSolutionSnippetDirectory(solution);

                if (Directory.Exists(snippetsDir))
                {
                    var snippetFolders = Directory.EnumerateDirectories(snippetsDir);

                    foreach (var folder in snippetFolders)
                    {
                        foreach (var file in Directory.EnumerateFiles(folder))
                        {
                            try
                            {
                                var snippet = SerializedObject.Deserialize<CodeSnippet>(file);
                                AddSnippet(_solutionSnippets[solution], Path.GetFileName(folder), snippet);
                            }
                            catch (Exception e)
                            {
                                IoC.Get<IConsole>().WriteLine($"Error parsing snippet: {file}, {e.Message}");
                            }
                        }
                    }
                }
            }
        }

        public void InitialiseSnippetsForProject(IProject project)
        {
            if (!_projectSnippets.ContainsKey(project) && !(project is UnresolvedReference))
            {
                _projectSnippets[project] = new Dictionary<string, IDictionary<string, CodeSnippet>>();

                var snippetsDir = Platform.GetProjectSnippetDirectory(project);

                if (Directory.Exists(snippetsDir))
                {
                    var snippetFolders = Directory.EnumerateDirectories(snippetsDir);

                    foreach (var folder in snippetFolders)
                    {
                        foreach (var file in Directory.EnumerateFiles(folder))
                        {
                            try
                            {
                                var snippet = SerializedObject.Deserialize<CodeSnippet>(file);
                                AddSnippet(_projectSnippets[project], Path.GetFileName(folder), snippet);
                            }
                            catch (Exception e)
                            {
                                IoC.Get<IConsole>().WriteLine($"Error parsing snippet: {file}, {e.Message}");
                            }
                        }
                    }
                }

                foreach (var reference in project.References)
                {
                    InitialiseSnippetsForProject(reference);
                }
            }
        }

        private void AddSnippet(Dictionary<string, IDictionary<string, CodeSnippet>> dictionary, string languageId, CodeSnippet snippet)
        {
            if (!dictionary.ContainsKey(languageId))
            {
                dictionary.Add(languageId, new Dictionary<string, CodeSnippet>());
            }

            if (!dictionary[languageId].ContainsKey(snippet.Name))
            {
                dictionary[languageId][snippet.Name] = snippet;
            }
        }

        private Dictionary<string, IDictionary<string, CodeSnippet>> _snippets = new Dictionary<string, IDictionary<string, CodeSnippet>>();

        private Dictionary<IProject, Dictionary<string, IDictionary<string, CodeSnippet>>> _projectSnippets = new Dictionary<IProject, Dictionary<string, IDictionary<string, CodeSnippet>>>();

        private Dictionary<ISolution, Dictionary<string, IDictionary<string, CodeSnippet>>> _solutionSnippets = new Dictionary<ISolution, Dictionary<string, IDictionary<string, CodeSnippet>>>();

        private CodeSnippet GetSnippetForProject(ILanguageService languageService, IProject project, string word)
        {
            if (!(project is UnresolvedReference))
            {
                if (_projectSnippets.ContainsKey(project) && _projectSnippets[project].ContainsKey(languageService.LanguageId))
                {
                    var projectSnippets = _projectSnippets[project][languageService.LanguageId];

                    if (projectSnippets.ContainsKey(word))
                    {
                        return projectSnippets[word];
                    }
                }

                foreach (var reference in project.References)
                {
                    var result = GetSnippetForProject(languageService, reference, word);

                    if (result != null)
                    {
                        return result;
                    }
                }

            }

            return null;
        }

        public CodeSnippet GetSnippet(ILanguageService languageService, ISolution solution, IProject project, string word)
        {
            var projectSnippetMatch = GetSnippetForProject(languageService, project, word);

            if (projectSnippetMatch != null)
            {
                return projectSnippetMatch;
            }

            if (_solutionSnippets.ContainsKey(solution) && _solutionSnippets[solution].ContainsKey(languageService.LanguageId))
            {
                var solutionSnippets = _solutionSnippets[solution][languageService.LanguageId];

                if (solutionSnippets.ContainsKey(word))
                {
                    return solutionSnippets[word];
                }
            }

            if (_snippets.ContainsKey(languageService.LanguageId) && _snippets[languageService.LanguageId].ContainsKey(word))
            {
                return _snippets[languageService.LanguageId][word];
            }

            return null;
        }

        private List<CodeSnippet> GetSnippetsForProject(ILanguageService languageService, IProject project)
        {
            var results = new List<CodeSnippet>();

            if (!(project is UnresolvedReference))
            {
                if (_projectSnippets.ContainsKey(project) && _projectSnippets[project].ContainsKey(languageService.LanguageId))
                {
                    foreach (var snippet in _projectSnippets[project][languageService.LanguageId].Values)
                    {
                        var currentResult = results.BinarySearch<CodeSnippet, CodeSnippet>(snippet);

                        if (currentResult == null)
                        {
                            results.Add(snippet);
                        }
                    }
                }

                foreach (var reference in project.References)
                {
                    results.AddRange(GetSnippetsForProject(languageService, reference));
                }
            }

            return results;
        }

        public List<CodeSnippet> GetSnippets(ILanguageService languageService, ISolution solution, IProject project)
        {
            var results = new List<CodeSnippet>();

            if (project != null)
            {
                results.AddRange(GetSnippetsForProject(languageService, project));
            }

            if (solution != null)
            {
                if (_solutionSnippets.ContainsKey(solution) && _solutionSnippets[solution].ContainsKey(languageService.LanguageId))
                {
                    foreach (var snippet in _solutionSnippets[solution][languageService.LanguageId].Values)
                    {
                        var currentResult = results.BinarySearch<CodeSnippet, CodeSnippet>(snippet);

                        if (currentResult == null)
                        {
                            results.Add(snippet);
                        }
                    }
                }
            }

            if (languageService != null)
            {
                if (_snippets.ContainsKey(languageService.LanguageId))
                {
                    foreach (var snippet in _snippets[languageService.LanguageId].Values)
                    {
                        var currentResult = results.BinarySearch<CodeSnippet, CodeSnippet>(snippet);

                        if (currentResult == null)
                        {
                            results.Add(snippet);
                        }
                    }
                }
            }

            return results;
        }
    }
}
