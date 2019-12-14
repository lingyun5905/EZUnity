/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-08-14 11:25:05
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZTransformTweenMixer : PlayableBehaviour
    {
        private bool started;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Transform targetTransform = playerData as Transform;
            if (targetTransform == null) return;

            Vector3 originalPosition = targetTransform.position;
            Quaternion originalRotation = targetTransform.rotation;
            Vector3 originalScale = targetTransform.localScale;

            Vector3 outputPosition = Vector3.zero;
            Quaternion outputRotation = new Quaternion(0f, 0f, 0f, 0f);
            Vector3 outputScale = Vector3.zero;

            float positionWeight = 0f;
            float rotationWeight = 0f;
            float scaleWeight = 0f;

            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                ScriptPlayable<EZTransformTweenBehaviour> inputPlayable = (ScriptPlayable<EZTransformTweenBehaviour>)playable.GetInput(i);
                EZTransformTweenBehaviour inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                if (!started && !inputBehaviour.startPoint)
                {
                    inputBehaviour.startPosition = originalPosition;
                    inputBehaviour.startRotation = originalRotation;
                    inputBehaviour.startScale = originalScale;
                }
                float normalizedTime = (float)(inputPlayable.GetTime() / inputPlayable.GetDuration());
                float process = inputBehaviour.curve.Evaluate(normalizedTime);

                if (inputBehaviour.tweenPosition && inputBehaviour.endPoint != null)
                {
                    positionWeight += inputWeight;
                    outputPosition += Vector3.Lerp(inputBehaviour.startPosition, inputBehaviour.endPoint.position, process) * inputWeight;
                }
                if (inputBehaviour.tweenRotation)
                {
                    if (inputBehaviour.endPoint != null)
                    {
                        rotationWeight += inputWeight;
                        Quaternion targetRotation = Quaternion.Lerp(inputBehaviour.startRotation, inputBehaviour.endPoint.rotation, process);
                        outputRotation = QuaternionExt.Cumulate(outputRotation, targetRotation.Scale(inputWeight));
                    }
                }
                if (inputBehaviour.tweenScale && inputBehaviour.endPoint != null)
                {
                    scaleWeight += inputWeight;
                    outputScale += Vector3.Lerp(inputBehaviour.startScale, inputBehaviour.endPoint.localScale, process) * inputWeight;
                }
            }
            if (positionWeight > 1e-5)
            {
                targetTransform.position = outputPosition + originalPosition * (1f - positionWeight);
            }
            if (rotationWeight > 1e-5)
            {
                targetTransform.rotation = QuaternionExt.Cumulate(outputRotation, originalRotation.Scale(1f - rotationWeight));
            }
            if (scaleWeight > 1e-5)
            {
                targetTransform.localScale = outputScale + originalScale * (1f - scaleWeight);
            }
            started = true;
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            started = false;
        }
    }
}
