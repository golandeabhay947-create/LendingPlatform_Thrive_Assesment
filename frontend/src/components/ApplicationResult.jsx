function ApplicationResult({ result }) {
  const decisionClass = result?.decision?.toLowerCase()

  return (
    <aside className="card result-card" aria-labelledby="result-title" aria-live="polite">
      <div>
        <p className="eyebrow">LATEST DECISION</p>
        <h3 id="result-title">Application outcome</h3>
        {!result && (
          <p className="result-placeholder">
            The lending decision and calculated loan-to-value ratio will appear here after submission.
          </p>
        )}
        {result && (
          <div className="result-detail">
            <span className={`decision-badge ${decisionClass}`}>{result.decision}</span>
            <p className="ltv-label">Loan-to-value</p>
            <p className="ltv-value">{result.ltv.toFixed(1)}%</p>
          </div>
        )}
      </div>
      <p className="result-footer">Decisions and LTV are returned by the lending service, not calculated in this interface.</p>
    </aside>
  )
}

export default ApplicationResult
