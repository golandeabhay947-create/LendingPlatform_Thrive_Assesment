import { useCallback, useEffect, useState } from 'react'
import './App.css'
import { getLoanStatistics } from './api/loansApi'
import ApplicationResult from './components/ApplicationResult'
import LoanApplicationForm from './components/LoanApplicationForm'
import StatisticsDashboard from './components/StatisticsDashboard'

function App() {
  const [statistics, setStatistics] = useState(null)
  const [statisticsError, setStatisticsError] = useState('')
  const [isStatisticsLoading, setIsStatisticsLoading] = useState(true)
  const [applicationResult, setApplicationResult] = useState(null)

  const refreshStatistics = useCallback(async () => {
    setIsStatisticsLoading(true)
    setStatisticsError('')

    try {
      setStatistics(await getLoanStatistics())
    } catch (error) {
      setStatisticsError(error.message)
    } finally {
      setIsStatisticsLoading(false)
    }
  }, [])

  useEffect(() => {
    refreshStatistics()
  }, [refreshStatistics])

  return (
    <div className="app-shell">
      <header className="site-header">
        <div className="header-content">
          <a className="brand" href="#application" aria-label="Lending Platform home">
            <span className="brand-mark" aria-hidden="true">LP</span>
            <span>
              <strong>Lending Platform</strong>
              <small>Credit decisions</small>
            </span>
          </a>
          <nav aria-label="Primary navigation">
            <a href="#application">New application</a>
            <a href="#statistics">Portfolio overview</a>
          </nav>
        </div>
      </header>

      <main>
        <section className="hero" aria-labelledby="page-title">
          <div>
            <p className="eyebrow">LENDING DECISION WORKSPACE</p>
            <h1 id="page-title">Clear credit decisions, built around your secured lending portfolio.</h1>
            <p className="hero-copy">
              Submit a lending application and receive a decision and loan-to-value ratio from the lending service.
            </p>
          </div>
          <div className="hero-detail" aria-label="Service status">
            <span className="status-dot" aria-hidden="true" />
            Lending decision service
          </div>
        </section>

        <section className="workspace" id="application" aria-labelledby="application-title">
          <div className="section-heading">
            <p className="eyebrow">NEW APPLICATION</p>
            <h2 id="application-title">Lending assessment</h2>
            <p>Enter the applicant and security details to request a lending decision.</p>
          </div>
          <div className="application-layout">
            <LoanApplicationForm
              onApplicationSubmitted={refreshStatistics}
              onResult={setApplicationResult}
            />
            <ApplicationResult result={applicationResult} />
          </div>
        </section>

        <section className="statistics-section" id="statistics" aria-labelledby="statistics-title">
          <div className="section-heading section-heading-row">
            <div>
              <p className="eyebrow">PORTFOLIO OVERVIEW</p>
              <h2 id="statistics-title">Application statistics</h2>
              <p>Current figures from applications held during this session.</p>
            </div>
            <button className="text-button" type="button" onClick={refreshStatistics} disabled={isStatisticsLoading}>
              {isStatisticsLoading ? 'Refreshing…' : 'Refresh figures'}
            </button>
          </div>
          <StatisticsDashboard
            statistics={statistics}
            isLoading={isStatisticsLoading}
            error={statisticsError}
          />
        </section>
      </main>

      <footer>
        <span>Lending Platform</span>
        <span>Secured lending decision workspace</span>
      </footer>
    </div>
  )
}

export default App
