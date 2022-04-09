using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
	UniverseMaster universemaster;

	private void OnEnable() {
		if (!SteamManager.Initialized)
			return;

		m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

		// These need to be reset to get the stats upon an Assembly reload in the Editor.
		m_bRequestedStats = false;
		m_bStatsValid = false;
	}

    private void Awake() {
		universemaster = FindObjectOfType<UniverseMaster>();
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
		if (pCallback.m_bActive != 0) {
			overworld_cattybatty catbat = FindObjectOfType<overworld_cattybatty>();
			if (catbat) {
				catbat.pauseGame();
				return;
            }

			DialogueManager dialogmanager = FindObjectOfType<DialogueManager>();
			if (dialogmanager) {
				dialogmanager.pauseGame();
				return;
            }

			Gamemaster gamemaster = FindObjectOfType<Gamemaster>();
			if (gamemaster) {
				gamemaster.pauseGame();
				return;
            }
		}
	}

	private enum Achievement : int
	{
		LEVEL_1,
		LEVEL_3,
		LEVEL_5,
		LEVEL_6,
		LEVEL_8,
		LEVEL_10,
		LEVEL_11,
		LEVEL_13,
		LEVEL_15,
		LEVEL_17,
		LEVEL_19,
		LEVEL_20,
		LEVEL_22,
		LEVEL_25,
		LEVEL_27,
		LEVEL_30,
	};

	private Achievement_t[] m_Achievements = new Achievement_t[] {
		new Achievement_t(Achievement.LEVEL_1, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_3, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_5, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_6, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_8, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_10, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_11, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_13, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_15, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_17, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_19, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_20, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_22, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_25, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_27, "It's Adventure Time", ""),
		new Achievement_t(Achievement.LEVEL_30, "It's Adventure Time", ""),
	};

	// Our GameID
	private CGameID m_GameID;

	// Did we get the stats from Steam?
	private bool m_bRequestedStats;
	private bool m_bStatsValid;

	// Should we store stats this frame?
	private bool m_bStoreStats;

	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	private void FixedUpdate() {
		if (!SteamManager.Initialized)
			return;

		if (!m_bRequestedStats) {
			// Is Steam Loaded? if no, can't get stats, done
			if (!SteamManager.Initialized) {
				m_bRequestedStats = true;
				return;
			}

			// If yes, request our stats
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}

		// Get info from sources
		// Evaluate achievements
		foreach (Achievement_t achievement in m_Achievements) {
			if (achievement.m_bAchieved)
				continue;

			switch (achievement.m_eAchievementID) {
				case Achievement.LEVEL_1:
				if (universemaster.observer.levels[0].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_3:
				if (universemaster.observer.levels[2].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_5:
				if (universemaster.observer.levels[4].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_6:
				if (universemaster.observer.levels[5].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_8:
				if (universemaster.observer.levels[7].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_10:
				if (universemaster.observer.levels[9].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_11:
				if (universemaster.observer.levels[10].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_13:
				if (universemaster.observer.levels[12].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_15:
				if (universemaster.observer.levels[14].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_17:
				if (universemaster.observer.levels[16].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_19:
				if (universemaster.observer.levels[18].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_20:
				if (universemaster.observer.levels[19].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_22:
				if (universemaster.observer.levels[21].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_25:
				if (universemaster.observer.levels[24].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_27:
				if (universemaster.observer.levels[26].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
				case Achievement.LEVEL_30:
				if (universemaster.observer.levels[29].total_times_finished > 0) {
					UnlockAchievement(achievement);
				}
				break;
			}
		}

		//Store stats in the Steam database if necessary
		if (m_bStoreStats) {
			// already set any achievements in UnlockAchievement

			bool bSuccess = SteamUserStats.StoreStats();
			// If this failed, we never sent anything to the server, try
			// again later.
			m_bStoreStats = !bSuccess;
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Unlock this achievement
	//-----------------------------------------------------------------------------
	private void UnlockAchievement(Achievement_t achievement) {
		achievement.m_bAchieved = true;

		// the icon may change once it's unlocked
		//achievement.m_iIconImage = 0;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
		SteamUserStats.StoreStats();

		// Store stats end of frame
		m_bStoreStats = true;

	}

	//-----------------------------------------------------------------------------
	// Purpose: We have stats data from Steam. It is authoritative, so update
	//			our data with those results now.
	//-----------------------------------------------------------------------------
	private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
		if (!SteamManager.Initialized)
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (EResult.k_EResultOK == pCallback.m_eResult) {
				Debug.Log("Received stats and achievements from Steam\n");

				m_bStatsValid = true;

				// load achievements
				foreach (Achievement_t ach in m_Achievements) {
					bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
					if (ret) {
						ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
					else {
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
					}
				}
			}
			else {
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Our stats data was stored!
	//-----------------------------------------------------------------------------
	private void OnUserStatsStored(UserStatsStored_t pCallback) {
	}

	//-----------------------------------------------------------------------------
	// Purpose: An achievement was stored
	//-----------------------------------------------------------------------------
	private void OnAchievementStored(UserAchievementStored_t pCallback) {
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (0 == pCallback.m_nMaxProgress) {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}

	private class Achievement_t
	{
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc) {
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}
}
