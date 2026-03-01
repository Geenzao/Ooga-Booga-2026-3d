using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaylistPlaying : MonoBehaviour
{
    private AudioSource _audioSource;
    //Liste de musiques joues
    public List<AudioClip> _listSongs;
    public List<AudioClip> _listGameOverSongs;
    //public List<AudioClip> _listGameWonSongs;
    public enum States
    {
        StateInGame,
        StateGameOver,
        StateGameWon
    }
    //L'tat dtermine quel playlist ce script doit jouer
    private States _state;
    private int _currentlyPlayingTrack = 0;
    private bool _ShouldBePlaying = true;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //Choix  l'utilisateur du groupe de mixage de sortie
        if (_listSongs != null)
            _audioSource.PlayOneShot(_listSongs[0]);
        GameStateManager.Instance.OnGameStatusUpdated += HandleGameStatusChanged;
    }

    private void HandleGameStatusChanged(GameStateManager.GameState currentPS, GameStateManager.GameState oldPS)
    {
        switch (currentPS)
        {
            case GameStateManager.GameState.DEAD:
            case GameStateManager.GameState.FIRED:
                SetState(States.StateGameOver);
                break;
            case GameStateManager.GameState.IN_OFFICE:
            case GameStateManager.GameState.IN_BOSS_OFFICE:
                SetState(States.StateInGame);
                break;
            //case GameStateManager.GameState.WINNER:
            //    SetState(States.StateGameWon);
            //    break;
        }
    }

    private void Update()
    {
        if (!_audioSource.isPlaying && _ShouldBePlaying)
        {
            switch (_state)
            {
                case States.StateInGame:
                    {
                        if (_currentlyPlayingTrack < _listSongs.Count - 1)
                            ++_currentlyPlayingTrack;
                        else
                            _currentlyPlayingTrack = 0;
                        _audioSource.PlayOneShot(_listSongs[_currentlyPlayingTrack]);
                        break;
                    }
                case States.StateGameOver:
                    {
                        if (_currentlyPlayingTrack < _listGameOverSongs.Count - 1)
                            ++_currentlyPlayingTrack;
                        else
                            _currentlyPlayingTrack = 0;
                        _audioSource.PlayOneShot(_listGameOverSongs[_currentlyPlayingTrack]);
                        break;
                    }
                //case States.StateGameWon:
                //    {
                //        if (_currentlyPlayingTrack < _listGameOverSongs.Count - 1)
                //            ++_currentlyPlayingTrack;
                //        else
                //            _currentlyPlayingTrack = 0;
                //        _audioSource.PlayOneShot(_listGameWonSongs[_currentlyPlayingTrack]);
                //        break;
                //    }
            }
        }
    }

    private void PauseMusic()
    {
        _ShouldBePlaying = !_ShouldBePlaying;

        if (!_ShouldBePlaying)
            _audioSource.Pause();
        else
            _audioSource.UnPause();
    }

    private void SetState(States toSetState)
    {
        if (_state == toSetState)
            return;
        _audioSource.Stop();
        _state = toSetState;
        _currentlyPlayingTrack = 0;
    }
}
