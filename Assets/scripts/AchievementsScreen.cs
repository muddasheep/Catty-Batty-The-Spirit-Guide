using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsScreen : MonoBehaviour
{
    public Animator[] achievement_animators;
    public Animator achievement_drop_down;
    public AchievementDisplayPicture[] pictures;
    UniverseMaster universemaster;

    // Start is called before the first frame update
    void Start()
    {
        achievement_animators = GetComponentsInChildren<Animator>();
        pictures = GetComponentsInChildren<AchievementDisplayPicture>();
    }

    public void ShowAchievements() {
        StartCoroutine(ShowAchievementIn(this));
    }

    public void HideAchievements() {
        StartCoroutine(ShowAchievementOut(this));
    }

    private IEnumerator ShowAchievementIn(AchievementsScreen self) {
        universemaster = FindObjectOfType<UniverseMaster>();
        foreach (AchievementDisplayPicture pic in pictures) {
            Achievement achievement = universemaster.observer.FindAchievementByID(pic.achievement_ID);
            if (achievement.achieved) {
                pic.GetComponentInChildren<SpriteRenderer>().sprite = pic.unlockedimage;
            }
        }

        self.achievement_drop_down.enabled = true;
        self.achievement_drop_down.StopPlayback();
        self.achievement_drop_down.Play("achievements_drop_down", -1, 0);
        Animator[] ars = reshuffle(self.achievement_animators);
        foreach (Animator ar in ars) {
            ar.enabled = true;
            ar.StopPlayback();
            ar.Play("achievement_fly_in", -1, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator ShowAchievementOut(AchievementsScreen self) {
        self.achievement_drop_down.enabled = true;
        self.achievement_drop_down.StopPlayback();
        self.achievement_drop_down.Play("achievements_drop_out", -1, 0);
        Animator[] ars = reshuffle(self.achievement_animators);
        foreach (Animator ar in ars) {
            ar.enabled = true;
            ar.StopPlayback();
            ar.Play("achievement_fly_out", -1, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public Animator[] reshuffle(Animator[] ars) {
        for (int t = 0; t < ars.Length; t++) {
            Animator tmp = ars[t];
            int r = Random.Range(t, ars.Length);
            ars[t] = ars[r];
            ars[r] = tmp;
        }
        return ars;
    }
}
