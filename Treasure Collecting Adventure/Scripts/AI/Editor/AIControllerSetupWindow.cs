using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using LupinrangerPatranger.InventorySystem;
using LupinrangerPatranger.StatSystem;
using UnityEngine.AI;

namespace LupinrangerPatranger
{
    public class AIControllerSetupWindow : EditorWindow
    {
        private GameObject m_Enemy;
        private AnimatorController m_AnimatorController;
        private NavMeshAgent m_NavMeshAgent;
        private AudioSource m_AudioSource;
        private PhysicMaterial m_MaxFriction;
        private PhysicMaterial m_Frictionless;
        private AudioMixerGroup m_AudioMixerGroup;
        private AudioClip m_FootstepClips;
        private bool m_DefaultMotions = true, m_CharacterIK = true, m_EquipmentHandler = true, m_StatsHandler = true, m_SingleInstance = true, m_SelectionHandler = true, m_AudioEventListener = true;

        [UnityEditor.MenuItem("Tools/Treasure Collecting Adventure/AI Controller/Setup Enemy", false, 0)]
        public static void ShowWindow()
        {
            AIControllerSetupWindow window = EditorWindow.GetWindow<AIControllerSetupWindow>("Enemy Setup");
            Vector2 size = new Vector2(300f, 100f);
            window.minSize = size;
            window.wantsMouseMove = true;
            window.m_AnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Treasure Collecting Adventure/Animations/Enemy/EnemyController.controller");
            //window.m_NavMeshAgent 
            window.m_MaxFriction = AssetDatabase.LoadAssetAtPath<PhysicMaterial>("Assets/Treasure Collecting Adventure/Third Person Controller/Example/Materials/MaxFriction.physicMaterial");
            window.m_Frictionless = AssetDatabase.LoadAssetAtPath<PhysicMaterial>("Assets/Treasure Collecting Adventure/Third Person Controller/Example/Materials/Frictionless.physicMaterial");
            window.m_AudioMixerGroup = AssetDatabase.LoadAssetAtPath<AudioMixerGroup>("Assets/Treasure Collecting Adventure/Inventory System/Example/Databases/Footsteps.mixer");
            window.m_FootstepClips = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Treasure Collecting Adventure/Third Person Controller/Example/Sounds/Footstep01.wav");
            window.m_FootstepClips = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Treasure Collecting Adventure/Third Person Controller/Example/Sounds/Footstep02.wav");
            window.m_FootstepClips = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Treasure Collecting Adventure/Third Person Controller/Example/Sounds/Footstep03.wav");
            window.m_FootstepClips = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Treasure Collecting Adventure/Third Person Controller/Example/Sounds/Footstep04.wav");
        }

        private void OnGUI()
        {
            if (m_Enemy == null)
            {
                EditorGUILayout.HelpBox("Select the GameObject which will be used as the enemy. The required components will be added to it.", MessageType.Error);
            }
            m_Enemy = (GameObject)EditorGUILayout.ObjectField("Enemy", m_Enemy, typeof(GameObject), true);
            if (m_Enemy == null)
            {
                return;
            }

            m_AnimatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", m_AnimatorController, typeof(AnimatorController), false);
            m_AudioMixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Databases", m_AudioMixerGroup, typeof(AudioMixerGroup), false);
            m_FootstepClips = (AudioClip)EditorGUILayout.ObjectField("Sounds", m_FootstepClips, typeof(AudioClip), false);
            m_DefaultMotions = EditorGUILayout.Toggle("Default Motions", m_DefaultMotions);
            m_CharacterIK = EditorGUILayout.Toggle("Character IK", m_CharacterIK);
            m_EquipmentHandler = EditorGUILayout.Toggle("Equipment Handler", m_EquipmentHandler);
            m_SelectionHandler = EditorGUILayout.Toggle("SelectionHandler", m_SelectionHandler);
            m_SingleInstance = EditorGUILayout.Toggle("SingleInstance", m_SingleInstance);
            m_AudioEventListener = EditorGUILayout.Toggle("Audio Event Listener", m_AudioEventListener);
            m_StatsHandler = EditorGUILayout.Toggle("Stats Handler", m_StatsHandler);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Build Enemy"))
            {
                if (EditorUtility.IsPersistent(m_Enemy))
                {
                    m_Enemy = Instantiate(m_Enemy);
                    Selection.activeObject = m_Enemy;
                }
                m_Enemy.layer = 15;
                m_Enemy.tag = "Enemy";
                SetupAnimatorController();
                SetupRigidbody();
                SetupCapsuleCollider();
                SetupEnemyController();
                SetupCharacterIK();
                SetupEquipmentHandler();
                SetupSelectionHandler();
                SetupSingleInstance();
                SetupAudioEventListener();
                SetupStatsHandler();
            }
        }

        private void SetupAnimatorController()
        {
            Animator animator = m_Enemy.GetComponent<Animator>();
            if (animator == null)
            {
                animator = m_Enemy.AddComponent<Animator>();
            }
            animator.runtimeAnimatorController = m_AnimatorController;
        }

        private void SetupRigidbody()
        {
            Rigidbody rigidbody = m_Enemy.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = m_Enemy.AddComponent<Rigidbody>();
            }
            rigidbody.mass = 1;
            rigidbody.drag = 0;
            rigidbody.angularDrag = 999;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void SetupCapsuleCollider()
        {
            CapsuleCollider collider = m_Enemy.GetComponent<CapsuleCollider>();
            if (collider == null)
            {
                collider = m_Enemy.AddComponent<CapsuleCollider>();
            }
            collider.isTrigger = false;
            collider.material = null;
            collider.center = new Vector3(0f, 0.9f, 0f);
            collider.radius = 0.25f;
            collider.height = 1.8f;
            collider.direction = 1;
        }

        private void SetupEnemyController()
        {
            EnemyController controller = m_Enemy.GetComponent<EnemyController>();
            if (controller == null)
            {
                controller = m_Enemy.AddComponent<EnemyController>();
            }
            //controller.IdleFriction = m_MaxFriction;
            //controller.MovementFriction = m_Frictionless;
            //controller.StepFriction = m_Frictionless;
            //controller.AirFriction = m_Frictionless;
            //controller.AudioMixerGroup = m_AudioMixerGroup;

            List<AudioClip> footstepclips = new List<AudioClip>();
            footstepclips.Add(m_FootstepClips);

            if (!m_DefaultMotions)
            {
                return;
            }
            List<MotionState> motions = new List<MotionState>();
            Swim swim = m_Enemy.AddComponent<Swim>();
            swim.State = "Swim";
            motions.Add(swim);

            Climb climbladder = m_Enemy.AddComponent<Climb>();
            climbladder.State = "Climb Ladder";
            climbladder.StartType = StartType.Automatic;
            climbladder.StopType = StopType.Manual;
            motions.Add(climbladder);

            Climb climb = m_Enemy.AddComponent<Climb>();
            climb.State = "Climb";
            climb.StartType = StartType.Automatic;
            climb.StopType = StopType.Manual;
            motions.Add(climb);

            Ladder ladder = m_Enemy.AddComponent<Ladder>();
            ladder.State = "Ladder";
            motions.Add(ladder);

            Fall fall = m_Enemy.AddComponent<Fall>();
            fall.State = "Fall";
            fall.StartType = StartType.Automatic;
            fall.StopType = StopType.Manual;
            motions.Add(fall);

            Slide slide = m_Enemy.AddComponent<Slide>();
            slide.State = "Slide";
            slide.InputName = "Crouch";
            slide.StartType = StartType.Down;
            slide.StopType = StopType.Toggle;
            slide.CameraPreset = "ChangeHeight";
            motions.Add(slide);

            ChangeHeight changeHeight = m_Enemy.AddComponent<ChangeHeight>();
            changeHeight.State = "Crouch";
            changeHeight.InputName = "Crouch";
            changeHeight.StartType = StartType.Down;
            changeHeight.StopType = StopType.Toggle;
            changeHeight.CameraPreset = "ChangeHeight";
            motions.Add(changeHeight);

            Jump jump = m_Enemy.AddComponent<Jump>();
            jump.State = "Jump";
            jump.InputName = "Jump";
            jump.StartType = StartType.Down;
            jump.StopType = StopType.Automatic;
            motions.Add(jump);

            Push push = m_Enemy.AddComponent<Push>();
            push.State = "Push";
            motions.Add(push);

            ChangeSpeed changeSpeed = m_Enemy.AddComponent<ChangeSpeed>();
            changeSpeed.InputName = "Change Speed";
            changeSpeed.StartType = StartType.Down;
            changeSpeed.StopType = StopType.Up;
            motions.Add(changeSpeed);

            SimpleMotion evade = m_Enemy.AddComponent<SimpleMotion>();
            evade.State = "Evade";
            evade.InputName = "Evade";
            evade.StartType = StartType.Down;
            evade.StopType = StopType.Manual;
            motions.Add(evade);

            Climb autojump = m_Enemy.AddComponent<Climb>();
            autojump.State = "Auto Jump";
            autojump.StartType = StartType.Automatic;
            autojump.StopType = StopType.Manual;
            motions.Add(autojump);

            //controller.Motions = motions;
        }

        private void SetupCharacterIK()
        {
            CharacterIK characterIK = m_Enemy.GetComponent<CharacterIK>();
            if (characterIK == null)
            {
                characterIK = m_Enemy.AddComponent<CharacterIK>();
            }

            if (!m_CharacterIK)
            {
                DestroyImmediate(characterIK);
            }
        }

        private void SetupEquipmentHandler()
        {
            EquipmentHandler equipmentHandler = m_Enemy.GetComponent<EquipmentHandler>();
            if (equipmentHandler == null)
            {
                equipmentHandler = m_Enemy.AddComponent<EquipmentHandler>();
            }
            if (!m_EquipmentHandler)
            {
                DestroyImmediate(equipmentHandler);
            }
        }

        private void SetupSelectionHandler()
        {
            SelectionHandler selectionHandler = m_Enemy.AddComponent<SelectionHandler>();
            if (m_Enemy == null)
            {
                selectionHandler = m_Enemy.AddComponent<SelectionHandler>();
            }
            if (!m_SelectionHandler)
            {
                DestroyImmediate(selectionHandler);
            }
        }
        private void SetupSingleInstance()
        {
            SingleInstance singleInstance = m_Enemy.AddComponent<SingleInstance>();
            if (m_Enemy == null)
            {
                singleInstance = m_Enemy.AddComponent<SingleInstance>();
            }
            if (!m_SingleInstance)
            {
                DestroyImmediate (singleInstance);
            }
        }

        private void SetupAudioEventListener()
        {
            AudioEventListener audioEventListener = m_Enemy.AddComponent<AudioEventListener>();
            if (m_Enemy == null)
            {
                audioEventListener = m_Enemy.AddComponent<AudioEventListener>();
            }
            if (!m_AudioEventListener)
            {
                DestroyImmediate((AudioEventListener)audioEventListener);
            }
        }
        private void SetupStatsHandler()
        {
            StatsHandler statsHandler = m_Enemy.GetComponent<StatsHandler>();
            if (statsHandler == null)
            {
                statsHandler = m_Enemy.AddComponent<StatsHandler>();
            }
            //statsHandler.HandlerName = "Player Stats";
            statsHandler.saveable = true;
            List<Stat> stats = new List<Stat>();
            

            if (!m_StatsHandler)
            {
                DestroyImmediate(statsHandler);
            }
        }
    }
}