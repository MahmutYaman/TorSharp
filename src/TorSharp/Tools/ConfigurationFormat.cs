﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Knapcode.TorSharp.Tools
{
    internal interface IConfigurationFormat
    {
        string UpdateLine(IDictionary<string, string> dictionary, string originalLine);
        string CreateLine(KeyValuePair<string, string> pair);
    }

    internal class ConfigurationFormat : IConfigurationFormat
    {
        public Regex CommentPattern => new Regex(@"^\s*#+");

        public string UpdateLine(IDictionary<string, string> dictionary, string originalLine)
        {
            // try to match the key
            var pieces = Regex.Split(originalLine.Trim(), @"\s+");
            if (pieces.Length < 2)
            {
                return originalLine;
            }

            string key = pieces[0];
            if (dictionary.TryGetValue(key, out var value))
            {
                var keyMatch = Regex.Match(originalLine, @"^(\s*)(?<Key>" + Regex.Escape(key) + @")(\s+)");
                dictionary.Remove(key);
                return CreateLine(new KeyValuePair<string, string>(keyMatch.Groups["Key"].Value, value));
            }

            return originalLine;
        }

        public string CreateLine(KeyValuePair<string, string> pair)
        {
            return $"{pair.Key} {pair.Value}";
        }
    }
}