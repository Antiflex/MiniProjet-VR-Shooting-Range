| Paramètre              | Valeur   | Justification                      |
| ---------------------- | -------- | ---------------------------------- |
| `N`                    | 32       | Bon compromis précision / coût     |
| `coneAngle`            | 60°      | Assez large pour éviter sans errer |
| `lookAheadBase`        | 2.0 m    | Anticipation obstacles             |
| `lookAheadSpeedFactor` | 1.0      | Plus vite → plus loin              |
| `clearanceRadius`      | 0.3 m    | Taille drone + marge               |
| `maxSpeed`             | 3.0 m/s  | Stable en VR                       |
| `maxAccel`             | 6.0 m/s² | Évite à-coups                      |
| `maxTurnRate`          | 180°/s   | Drone agile mais crédible          |
| `wSafe`                | 3.0      | Sécurité prioritaire               |
| `wFollow`              | 1.0      | Suivi naturel                      |
| `wLoS`                 | 0.5      | Visibilité secondaire              |
| `wDyn`                 | 1.5      | Anti-jitter                        |
