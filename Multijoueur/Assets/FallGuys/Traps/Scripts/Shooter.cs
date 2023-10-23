using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject turret; // Object physique représentant la tourelle
    private Transform targetPosition; // Référence au joueur ciblé.
    private GameObject target;

    // Angle de rotation et rayon du secteur de détection
    public float rangeZone = 10f; // Taille du rayon de la zone de détection du joueur
    public float rotationAngle = 180f; // Angle maximal
    private SphereCollider detectionCollider; // Collider récupérant les joueurs entrant

    public float detectionAngle = 30f; // Angle de détection du joueur.
    public float rotationSpeed = 10f; // Vitesse de rotation.

    // Liste des cibles dans la zone.
    private List<GameObject> targetsInZone = new List<GameObject>();

    private enum ShooterState
    {
        Search,
        Orientation,
        Fire
    }

    private ShooterState currentState;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    // Paramètre de l'état de tire
    private float lastShotTime;
    public float shootCooldown = 1f;
    public float projectileDuration = 1.5f;

    private void Start()
    {
        currentState = ShooterState.Search;

        // Obtenez le composant SphereCollider attaché à cet objet.
        detectionCollider = GetComponent<SphereCollider>();

        // Assurez-vous que le SphereCollider existe.
        if (detectionCollider != null)
        {
            // Affectez la valeur de "range" à la propriété "radius" du SphereCollider.
            detectionCollider.radius = rangeZone;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case ShooterState.Search:
                SearchForTarget();
                break;
            case ShooterState.Orientation:
                OrientTowardsTarget();
                break;
            case ShooterState.Fire:
                FireAtTarget();
                break;
        }
    }

    // Etat de recherche
    private void SearchForTarget()
    {
        //Debug.Log(target);
        // Calculez l'angle actuel en fonction du temps et de la vitesse de rotation.
        float currentAngle = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle);

        // Si l'angle actuel dépasse la moitié de l'angle de rotation, inversez la rotation.
        if (currentAngle > rotationAngle / 2)
        {
            currentAngle = rotationAngle - currentAngle;
        }

        // Appliquez la rotation autour de l'axe Y.
        turret.transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);

        // Code pour détecter la présence du joueur dans la zone de détection en pointillé bleu.
        // Si un joueur est détecté, passez à l'état d'orientation.
        if (FindPlayerInSector())
        {
            currentState = ShooterState.Orientation;
        }
    }


    private bool FindPlayerInSector()
    {
        foreach (GameObject playerTransform in targetsInZone)
        {
            if (PlayerInSector(playerTransform.transform))
            {
                // Le joueur est dans le secteur d'angle, retournez-le comme nouvelle cible.
                target = playerTransform;
                targetPosition = playerTransform.transform;
                return true;
            }
        }

        // Aucun joueur dans le secteur, retournez null.
        return false;
    }

    // Etat d'orientation

    private void OrientTowardsTarget()
    {
        //Debug.Log(target);
        // Vérifiez si la cible est toujours dans le secteur et dans la liste des cibles.
        if (target == null || !PlayerInSector(targetPosition) || !targetsInZone.Contains(target))
        {
            currentState = ShooterState.Search;
            return;
        }

        // Effectuer la rotation
        RotateTowardsTarget();

        // Si la cible est dans l'axe de tir, passez à l'état de tir.
        if (PlayerInFireAngle())
        {
            currentState = ShooterState.Fire;
        }
    }

    private void RotateTowardsTarget()
    {
        // Obtenez la direction vers la cible uniquement autour de l'axe Y.
        Vector3 directionToTarget = targetPosition.position - turret.transform.position;
        directionToTarget.y = 0f;

        if (directionToTarget != Vector3.zero)
        {
            // Calculez la rotation uniquement autour de l'axe Y.
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            // Appliquez la rotation autour de l'axe Y.
            turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // Etat de mise à feu

    private bool PlayerInFireAngle()
    {
        // Vérifiez si le joueur est dans l'angle de tir du lanceur.
        if (target != null && PlayerInSector(targetPosition) && targetsInZone.Contains(target))
        {
            Vector3 directionToTarget = (targetPosition.position - turret.transform.position).normalized;
            float angle = Vector3.Angle(turret.transform.forward, directionToTarget);
            return angle <= detectionAngle / 2;
        }
        return false;
    }

    private void FireAtTarget()
    {
        // La tourelle continue à s'axer sur le joueur
        RotateTowardsTarget();

        if (PlayerInFireAngle())
        {
            // Code pour tirer sur le joueur.
            if (Time.time - lastShotTime >= shootCooldown)
            {
                if (projectilePrefab != null && projectileSpawnPoint != null)
                {
                    // Créez une instance du projectile.
                    GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

                    // Obtenez la vecteur direction de tir (utilisez seulement x et z).
                    Vector3 shootDirection = new Vector3(projectileSpawnPoint.forward.x, 0f, projectileSpawnPoint.forward.z);

                    // Appliquez la vitesse du projectile uniquement sur les axes x et z.
                    bullet.GetComponent<Rigidbody>().velocity = 30.0f * shootDirection;

                    lastShotTime = Time.time;
                    // Détruisez le projectile après un certain délai.
                    Destroy(bullet, projectileDuration);
                }
            }
        }
        else
        {
            currentState = ShooterState.Orientation;
        }
    }

    private bool PlayerInSector(Transform playerTransform)
    {
        // Calcule la direction du joueur par rapport au point central du secteur
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Calcule l'angle entre la direction du joueur et le vecteur avant du secteur
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Vérifie si l'angle est inférieur à la moitié de l'angle du secteur
        if (angleToPlayer < 180 / 2.0f)
        {
            // Le joueur se trouve à l'intérieur du secteur circulaire
            return true;
        }
        else
        {
            // Le joueur est à l'extérieur du secteur circulaire
            return false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assurez-vous d'utiliser la balise (tag) appropriée pour les joueurs.
        {
            // Transform playerTransform = other.transform;
            GameObject playerGameObject = other.gameObject;
            if (!targetsInZone.Contains(playerGameObject))
            {
                targetsInZone.Add(playerGameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Transform playerTransform = other.transform;
            GameObject playerGameObject = other.gameObject;
            if (targetsInZone.Contains(playerGameObject))
            {
                targetsInZone.Remove(playerGameObject);
            }
        }
    }
}

