using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomPhysics
{
    // Mueve el Transform en una direcci�n y velocidad espec�ficas.
    public static void MoveTransform(Transform transform, float speed, Vector2 direction)
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    // M�todo de colisi�n entre dos rect�ngulos (optimizando las comparaciones).
    public static bool RectangleCollision(Collider rec1, Collider rec2)
    {
        Bounds b1 = rec1.bounds;
        Bounds b2 = rec2.bounds;

        return b1.min.x < b2.max.x &&
               b1.max.x > b2.min.x &&
               b1.min.y < b2.max.y &&
               b1.max.y > b2.min.y;
    }

    // M�todo de colisi�n entre un rect�ngulo y un c�rculo.
    public static bool SphereRectangleCollision(Collider rectangle, SphereCollider sphere)
    {
        // Obtener la posici�n y el tama�o de ambos objetos
        Vector3 spherePosition = sphere.transform.position;
        float sphereRadius = sphere.radius * sphere.transform.lossyScale.x;

        Vector3 rectCenter = rectangle.bounds.center;
        Vector3 rectSize = rectangle.bounds.size;

        // Solo nos interesa X para posicionamiento horizontal
        float testX = Mathf.Clamp(spherePosition.x, rectCenter.x - rectSize.x / 2, rectCenter.x + rectSize.x / 2);

        // Y para la vertical, chequeamos la distancia real
        float testY = Mathf.Clamp(spherePosition.y, rectCenter.y - rectSize.y / 2, rectCenter.y + rectSize.y / 2);

        // Ahora calculamos distancias
        float distanceX = spherePosition.x - testX;
        float distanceY = spherePosition.y - testY;

        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

        // Solo hay colisi�n si realmente estamos dentro del radio
        return distanceSquared <= (sphereRadius * sphereRadius);
    }







    // Colisi�n entre dos esferas.
    public static bool SphereCollision(SphereCollider sphere1, SphereCollider sphere2)
    {
        float combinedRadius = sphere1.radius * sphere1.transform.lossyScale.x + sphere2.radius * sphere2.transform.lossyScale.x;
        return Vector2.Distance(sphere1.transform.position, sphere2.transform.position) < combinedRadius;
    }

    // C�lculo de la direcci�n reflejada en un rebote.
    public static Vector2 ReflectDirection(Vector2 direction, Vector2 normal)
    {
        return direction - 2 * Vector2.Dot(direction, normal) * normal;
    }



    // M�todo para aplicar una fuerza en un objeto seg�n su masa
    public static Vector2 ApplyForce(Vector2 velocity, Vector2 force, float mass)
    {
        return velocity + force / mass;
    }

    // Limita la velocidad a un valor m�ximo
    public static Vector2 LimitSpeed(Vector2 velocity, float maxSpeed)
    {
        if (velocity.magnitude > maxSpeed)
        {
            return velocity.normalized * maxSpeed;
        }
        return velocity;
    }

    // Aplica un rebote en funci�n de un coeficiente de restituci�n
    public static Vector2 CalculateBounce(Vector2 velocity, float restitution)
    {
        return velocity * -restitution;
    }


    public static Vector2 CalculatePaddleBounce(Vector2 ballPosition, Transform paddleTransform, float velocityMagnitude, float restitution, float ballScaleX, float paddleScaleX)
    {
        // Calcular el punto de impacto relativo entre el centro de la bola y el paddle ajustado a las escalas
        float impactPoint = (ballPosition.x - paddleTransform.position.x) / ballScaleX;
        float normalizedImpact = impactPoint / ((paddleTransform.GetComponent<Collider>().bounds.size.x * paddleScaleX) / 2);

        // Crear la direcci�n de rebote usando el impacto relativo y la magnitud de la velocidad
        Vector2 bounceDirection = new Vector2(normalizedImpact, 1).normalized;
        return bounceDirection * velocityMagnitude * restitution;
    }



}

