Function MetricProcessor
{
    Param([LogMonitor::FileChange] $change)
    Process
    {
        If ($change -eq null)
        {
            Throw "No change specified."
        }
        
        $metrics = @()
        
        if (-not $change.GetType().FullName -eq "LogMonitor.Processors.W3CChange")
        {
            return $metrics
        }
        
        $http_1xx = 0
        $http_2xx = 0
        $http_3xx = 0
        $http_4xx = 0
        $http_5xx = 0
        
        $statusIndex = [array]::indexof($change.FieldNames,[LogMonitor::Processors::W3CFields]::ScStatus)
        $timeTakenIndex = [array]::indexof($change.FieldNames,[LogMonitor::Processors::W3CFields]::TimeTaken)
        
        foreach ($values in $change.Values)
        {
            if ($statusIndex -ge 0)
            {
                $status = [int]$values[$statusIndex]
            
                if ($status < 200) { $http_1xx += 1 }
                ElseIf ($status < 300) { $http_2xx += 1 }
                ElseIf ($status < 400) { $http_3xx += 1 }
                ElseIf ($status < 500) { $http_4xx += 1 }
                Else { $http_5xx += 1 }
            }
            
            if ($timeTakenIndex -ge 0) 
            {
                $metrics += [LogMonitor::Processors::Metric]::Create('time_taken', [float]$values[$timeTakenIndex], 'timing')
            }
        }
        
        $metrics += [LogMonitor::Processors::Metric]::Create('http_1xx', [float]$http_1xx, 'counter')
        $metrics += [LogMonitor::Processors::Metric]::Create('http_2xx', [float]$http_2xx, 'counter')
        $metrics += [LogMonitor::Processors::Metric]::Create('http_3xx', [float]$http_3xx, 'counter')
        $metrics += [LogMonitor::Processors::Metric]::Create('http_4xx', [float]$http_4xx, 'counter')
        $metrics += [LogMonitor::Processors::Metric]::Create('http_5xx', [float]$http_5xx, 'counter')
        
        $metrics += [LogMonitor::Processors::Metric]::Create('calls', $change.Values.length, 'counter')
        
        return $metrics
    }
}