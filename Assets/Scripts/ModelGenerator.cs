using UnityEngine;

public static class ModelGenerator
{
    public static GameObject CreateTrainWagon(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject wagon = new GameObject("TrainWagon");
        if (parent != null) wagon.transform.SetParent(parent);
        wagon.transform.position = position;
        wagon.transform.rotation = rotation;

        // Main body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Body";
        body.transform.SetParent(wagon.transform);
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(2.8f, 2.5f, 10f);
        body.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.3f, 0.6f);
        body.tag = "Obstacle";

        // Roof
        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.name = "Roof";
        roof.transform.SetParent(wagon.transform);
        roof.transform.localPosition = new Vector3(0f, 1.4f, 0f);
        roof.transform.localScale = new Vector3(3f, 0.3f, 10.2f);
        roof.GetComponent<MeshRenderer>().material.color = new Color(0.15f, 0.2f, 0.4f);

        // Windows
        for (int i = -3; i <= 3; i++)
        {
            GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
            window.name = $"Window_{i}";
            window.transform.SetParent(wagon.transform);
            window.transform.localPosition = new Vector3(1.41f, 0.5f, i * 1.2f);
            window.transform.localScale = new Vector3(0.1f, 0.8f, 0.8f);
            window.GetComponent<MeshRenderer>().material.color = new Color(0.7f, 0.9f, 1f);

            GameObject windowL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            windowL.name = $"WindowL_{i}";
            windowL.transform.SetParent(wagon.transform);
            windowL.transform.localPosition = new Vector3(-1.41f, 0.5f, i * 1.2f);
            windowL.transform.localScale = new Vector3(0.1f, 0.8f, 0.8f);
            windowL.GetComponent<MeshRenderer>().material.color = new Color(0.7f, 0.9f, 1f);
        }

        // Wheels
        for (int side = -1; side <= 1; side += 2)
        {
            for (int z = -3; z <= 3; z += 6)
            {
                GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wheel.name = $"Wheel_{side}_{z}";
                wheel.transform.SetParent(wagon.transform);
                wheel.transform.localPosition = new Vector3(side * 1.6f, -1.2f, z);
                wheel.transform.localScale = new Vector3(0.8f, 0.3f, 0.8f);
                wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                wheel.GetComponent<MeshRenderer>().material.color = new Color(0.1f, 0.1f, 0.1f);
            }
        }

        // Headlight
        GameObject headlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        headlight.name = "Headlight";
        headlight.transform.SetParent(wagon.transform);
        headlight.transform.localPosition = new Vector3(0f, 0.5f, 5.1f);
        headlight.transform.localScale = new Vector3(0.6f, 0.6f, 0.3f);
        headlight.GetComponent<MeshRenderer>().material.color = Color.yellow;

        // Add collider to main body for physics
        BoxCollider col = body.GetComponent<BoxCollider>();
        if (col == null) col = body.AddComponent<BoxCollider>();

        return wagon;
    }

    public static GameObject CreatePlayerCharacter(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject character = new GameObject("PlayerCharacter");
        if (parent != null) character.transform.SetParent(parent);
        character.transform.position = position;
        character.transform.rotation = rotation;

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Body";
        body.transform.SetParent(character.transform);
        body.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        body.transform.localScale = new Vector3(0.6f, 0.7f, 0.3f);
        body.GetComponent<MeshRenderer>().material.color = new Color(0.9f, 0.2f, 0.2f);

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.SetParent(character.transform);
        head.transform.localPosition = new Vector3(0f, 1.1f, 0f);
        head.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        head.GetComponent<MeshRenderer>().material.color = new Color(1f, 0.8f, 0.6f);

        // Cap
        GameObject cap = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cap.name = "Cap";
        cap.transform.SetParent(character.transform);
        cap.transform.localPosition = new Vector3(0f, 1.35f, 0.1f);
        cap.transform.localScale = new Vector3(0.55f, 0.15f, 0.55f);
        cap.GetComponent<MeshRenderer>().material.color = new Color(0.1f, 0.1f, 0.8f);

        // Arms
        for (int side = -1; side <= 1; side += 2)
        {
            GameObject arm = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arm.name = $"Arm_{side}";
            arm.transform.SetParent(character.transform);
            arm.transform.localPosition = new Vector3(side * 0.4f, 0.6f, 0f);
            arm.transform.localScale = new Vector3(0.15f, 0.5f, 0.15f);
            arm.GetComponent<MeshRenderer>().material.color = new Color(1f, 0.8f, 0.6f);
        }

        // Legs
        for (int side = -1; side <= 1; side += 2)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.name = $"Leg_{side}";
            leg.transform.SetParent(character.transform);
            leg.transform.localPosition = new Vector3(side * 0.15f, -0.2f, 0f);
            leg.transform.localScale = new Vector3(0.18f, 0.6f, 0.18f);
            leg.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.5f);
        }

        // Backpack
        GameObject backpack = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backpack.name = "Backpack";
        backpack.transform.SetParent(character.transform);
        backpack.transform.localPosition = new Vector3(0f, 0.6f, -0.25f);
        backpack.transform.localScale = new Vector3(0.5f, 0.5f, 0.2f);
        backpack.GetComponent<MeshRenderer>().material.color = new Color(0.4f, 0.2f, 0.1f);

        return character;
    }

    public static GameObject CreateCoin(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        coin.name = "Coin";
        if (parent != null) coin.transform.SetParent(parent);
        coin.transform.position = position;
        coin.transform.rotation = rotation;
        coin.transform.localScale = new Vector3(0.6f, 0.1f, 0.6f);
        coin.GetComponent<MeshRenderer>().material.color = Color.yellow;
        coin.tag = "Coin";

        // Add rotation animator
        var rotator = coin.AddComponent<Rotator>();
        rotator.rotationSpeed = 180f;

        return coin;
    }

    public static GameObject CreateObstacle(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.name = "Obstacle";
        if (parent != null) obstacle.transform.SetParent(parent);
        obstacle.transform.position = position;
        obstacle.transform.rotation = rotation;
        obstacle.transform.localScale = new Vector3(2f, 2f, 2f);
        obstacle.GetComponent<MeshRenderer>().material.color = new Color(0.4f, 0.25f, 0.1f);
        obstacle.tag = "Obstacle";
        return obstacle;
    }

    public static GameObject CreateTrackSegment(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject segment = new GameObject("TrackSegment");
        if (parent != null) segment.transform.SetParent(parent);
        segment.transform.position = position;
        segment.transform.rotation = rotation;
        segment.tag = "Ground";

        // Main platform
        GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.name = "Platform";
        platform.transform.SetParent(segment.transform);
        platform.transform.localPosition = Vector3.zero;
        platform.transform.localScale = new Vector3(11f, 0.5f, 30f);
        platform.GetComponent<MeshRenderer>().material.color = new Color(0.35f, 0.35f, 0.35f);
        platform.tag = "Ground";

        // Lane dividers
        for (int i = -1; i <= 1; i++)
        {
            if (i == 0) continue;
            GameObject divider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            divider.name = $"Divider_{i}";
            divider.transform.SetParent(segment.transform);
            divider.transform.localPosition = new Vector3(i * 3f, 0.26f, 0f);
            divider.transform.localScale = new Vector3(0.1f, 0.05f, 30f);
            divider.GetComponent<MeshRenderer>().material.color = new Color(0.9f, 0.9f, 0.9f);
        }

        // Side rails
        for (int side = -1; side <= 1; side += 2)
        {
            GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.name = $"Rail_{side}";
            rail.transform.SetParent(segment.transform);
            rail.transform.localPosition = new Vector3(side * 5.5f, 0.5f, 0f);
            rail.transform.localScale = new Vector3(0.3f, 1f, 30f);
            rail.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
        }

        return segment;
    }
}

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 180f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
