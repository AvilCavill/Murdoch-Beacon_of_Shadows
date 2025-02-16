using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsMenu : MonoBehaviour
    {
        public TMP_Dropdown resolutionDropdown;
        public Toggle vSyncToggle;
        public Slider volumeSlider; // Slider para el volumen
        public AudioSource menuMusicSource; // Audio del menú principal
        private List<Resolution> filteredResolutions;
        private const string VolumeKey = "MenuVolume"; // Clave para guardar el volumen en PlayerPrefs

        private void Start()
        {
            // Obtener todas las resoluciones disponibles sin duplicados
            Resolution[] allResolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            HashSet<string> addedResolutions = new HashSet<string>();

            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < allResolutions.Length; i++)
            {
                string resolutionString = allResolutions[i].width + " x " + allResolutions[i].height;

                if (!addedResolutions.Contains(resolutionString))
                {
                    addedResolutions.Add(resolutionString);
                    filteredResolutions.Add(allResolutions[i]);
                    options.Add(resolutionString);

                    if (allResolutions[i].width == Screen.currentResolution.width &&
                        allResolutions[i].height == Screen.currentResolution.height)
                    {
                        currentResolutionIndex = filteredResolutions.Count - 1;
                    }
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            // Configuración de VSync
            vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
            vSyncToggle.onValueChanged.AddListener(SetVSync);

            // Configuración de Volumen
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
            menuMusicSource.volume = savedVolume;

            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume;
                volumeSlider.onValueChanged.AddListener(SetVolume);
            }
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void SetVSync(bool isEnabled)
        {
            QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        }

        public void SetVolume(float volume)
        {
            Debug.Log("Nuevo volumen: " + volume);
            menuMusicSource.volume = volume;
            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save();
        }
    }
}
