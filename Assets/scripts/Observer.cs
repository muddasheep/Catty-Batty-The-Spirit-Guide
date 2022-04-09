using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Observer
{
    /* This class should be easy to store and restore, to save and retrieve progress. */

    public List<SpiritGuideLevel> levels = new List<SpiritGuideLevel>();
    public List<OptionSetting> options = new List<OptionSetting>();
    public List<Achievement> achievements = new List<Achievement>();
    public OptionSetting[] optionsettings;
    public int total_levels = 62;
    public int last_played_level_number = 0;
    public int highest_played_level_number = 0;
    public int catty_style_number = 0;
    public int batty_style_number = 0;
    public bool player_control_switched = false;

    public void initialize_achievements() {
        if (achievements.Count > 0) {
            return;
        }

        achievements.Add(new Achievement(new_ID: "LEVEL_1"));
        achievements.Add(new Achievement(new_ID: "LEVEL_3"));
        achievements.Add(new Achievement(new_ID: "LEVEL_5"));
        achievements.Add(new Achievement(new_ID: "LEVEL_6"));
        achievements.Add(new Achievement(new_ID: "LEVEL_8"));
        achievements.Add(new Achievement(new_ID: "LEVEL_10"));
        achievements.Add(new Achievement(new_ID: "LEVEL_11"));
        achievements.Add(new Achievement(new_ID: "LEVEL_13"));
        achievements.Add(new Achievement(new_ID: "LEVEL_15"));
        achievements.Add(new Achievement(new_ID: "LEVEL_17"));
        achievements.Add(new Achievement(new_ID: "LEVEL_19"));
        achievements.Add(new Achievement(new_ID: "LEVEL_20"));
        achievements.Add(new Achievement(new_ID: "LEVEL_22"));
        achievements.Add(new Achievement(new_ID: "LEVEL_25"));
        achievements.Add(new Achievement(new_ID: "LEVEL_27"));
        achievements.Add(new Achievement(new_ID: "LEVEL_30"));
    }

    public void SetAchievementAchieved(string ID) {
        Achievement achievement = FindAchievementByID(ID);
        if (achievement == null) {
            return;
        }
        achievement.achieved = true;
    }

    public Achievement FindAchievementByID(string ID) {
        foreach (Achievement achievement in achievements) {
            if (achievement.ID == ID) {
                return achievement;
            }
        }
        return null;
    }

    public void initialize_levels() {
        if (levels.Count == total_levels) {
            return;
        }

        for (int i = levels.Count; i < total_levels; i++) {
            SpiritGuideLevel level = new SpiritGuideLevel();
            level.level_number = i + 1;
            levels.Add(level);
        }
    }

    public SpiritGuideLevel GetLevelByIndex(int i) {
        return levels[i];
    }

    public OptionSetting FindOption(string name) {
        OptionSetting found = options.Find(x => x.property_name == name);
        return found;
    }

    public void AddOption(OptionSetting new_option) {
        OptionSetting found = FindOption(new_option.property_name);

        if (found != null) {
            found.property_value = new_option.property_value;
        }
        else {
            options.Add(new_option);
        }
    }

    public void UpdateOptionsArray() {
        optionsettings = options.ToArray();
    }
}

[System.Serializable]
public class OptionSetting
{
    public string property_name;
    public int property_value;

    public OptionSetting(string name, int value) {
        property_name = name;
        property_value = value;
    }
}
