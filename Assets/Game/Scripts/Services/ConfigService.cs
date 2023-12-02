using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ConfigService {
    private ResourceLoaderService _resourceLoader;
    public List<LevelConfig> LevelConfigs { get; private set; }

    [Inject]
    public ConfigService(
    ResourceLoaderService resourceLoader
    ) {
        _resourceLoader = resourceLoader;
    }

    public void LoadConfigs() {
        LevelConfigs = LoadLevelConfig();
    }

    private List<LevelConfig> LoadLevelConfig() {
        TextAsset fileRaw = _resourceLoader.GetConfigFile("LevelConfig");
        string[,] fileGrid = CsvParserService.SplitCsvGrid(fileRaw.text);

        List<LevelConfig> configs = new List<LevelConfig>();
        for (int y = 1; y < fileGrid.GetLength(1); y++) {
            if (string.IsNullOrEmpty(fileGrid[1, y])) {
                continue;
            }

            int StartValue = int.Parse(fileGrid[1, y]);
            int EndValue = int.Parse(fileGrid[2, y]);
            int Level = int.Parse(fileGrid[0, y]);

            configs.Add(new LevelConfig {
                StartValue = StartValue,
                EndValue = EndValue,
                Level = Level,
            });
        }
        return configs;
    }
}

//float timeSpawn = float.Parse(fileGrid[0, y], CultureInfo.InvariantCulture);
//float moveTimeFromStartPointToTarget = float.Parse(fileGrid[1, y], CultureInfo.InvariantCulture);
//float moveTimeFromFirstTargetToAnyTarget = float.Parse(fileGrid[2, y], CultureInfo.InvariantCulture);
//float moveTimeFromCassToCar = float.Parse(fileGrid[3, y], CultureInfo.InvariantCulture);
//int carPrice = int.Parse(fileGrid[4, y], CultureInfo.InvariantCulture);