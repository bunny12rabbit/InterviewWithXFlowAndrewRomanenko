namespace InterviewXFlowAndrewRomanenko.Cleanup
{
    internal class ProgramRefactored
    {
        private const double TargetChangeTime = 1.0;

        private double _previousTargetSetTime;
        private bool _isTargetSet;
        private ITargetable _lockedCandidateTarget;
        private ITargetable _lockedTarget;
        private ITargetable _activeTarget;
        private TargetInRangeContainer _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            CleanupInvalidTargets();
            TrySetActiveTargetFromQuantum(frame);

            if (TrySetTarget())
                UpdateTargetSetTime();
            else
                ClearTarget();
        }

        private void CleanupInvalidTargets()
        {
            _lockedCandidateTarget = CleanupTarget(_lockedCandidateTarget);
            _lockedTarget = CleanupTarget(_lockedTarget);
        }

        private ITargetable CleanupTarget(ITargetable target) => target?.CanBeTarget == true ? target : null;

        private bool TrySetTarget() => 
            TrySetTargetFromPrevious() || TrySetTargetFromLocked() || TrySetTargetFromActive() || TrySetTargetFromRange());

        private bool TrySetTargetFromPrevious()
        {
            if (IsValidTarget(_lockedTarget))
            {
                SetTarget(_lockedTarget);
                return true;
            }

            return false;
        }

        private bool TrySetTargetFromLocked()
        {
            if (IsValidTarget(_activeTarget))
            {
                SetTarget(_activeTarget);
                return true;
            }

            return false;
        }

        private bool TrySetTargetFromActive()
        {
            var target = _targetInRangeContainer.GetTarget();

            if (IsValidTarget(target))
            {
                SetTarget(target);
                return true;
            }

            return false;
        }

        private bool IsValidTarget(ITargetable target) => target != null && target.CanBeTarget;

        private void SetTarget(ITargetable target)
        {
            _target = target;
            _isTargetSet = true;
        }

        private void ClearTarget()
        {
            _target = null;
            TargetableEntity.Selected = null;
        }

        private void UpdateTargetSetTime()
        {
            if (_previousTarget != _target)
                _previousTargetSetTime = Time.time;

            TargetableEntity.Selected = _target;
        }
        
        internal interface ITargetable
        {
            bool CanBeTarget { get; }
        }
        
        // Пример реализации ITargetable
        internal class Enemy : ITargetable
        {
            public bool CanBeTarget { get; set; }
            // MORE CLASS CODE
        }

        internal class TargetInRangeContainer
        {
            public ITargetable GetTarget()
            {
                // реализация получения цели 
                return null; // Placeholder
            }
        }

        // MORE CLASS CODE
    }
}