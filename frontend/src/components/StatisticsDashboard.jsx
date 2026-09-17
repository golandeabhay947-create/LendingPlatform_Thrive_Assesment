const currencyFormatter = new Intl.NumberFormat('en-GB', {
  style: 'currency',
  currency: 'GBP',
  maximumFractionDigits: 0,
})

function StatisticsDashboard({ statistics, isLoading, error }) {
  if (error) {
    return <p className="stats-error" role="alert">{error}</p>
  }

  const values = statistics || {
    successfulApplications: 0,
    declinedApplications: 0,
    totalLoanValue: 0,
    meanLtv: 0,
  }

  const cards = [
    ['Successful applications', values.successfulApplications],
    ['Declined applications', values.declinedApplications],
    ['Total loan value', currencyFormatter.format(values.totalLoanValue)],
    ['Mean LTV', `${Number(values.meanLtv).toFixed(1)}%`],
  ]

  return (
    <div aria-busy={isLoading}>
      <div className="statistics-grid">
        {cards.map(([label, value]) => (
          <article className="stat-card" key={label}>
            <p>{label}</p>
            <strong>{isLoading ? '—' : value}</strong>
          </article>
        ))}
      </div>
      {!isLoading && values.successfulApplications + values.declinedApplications === 0 && (
        <p className="stats-note">No applications have been submitted in this session yet.</p>
      )}
    </div>
  )
}

export default StatisticsDashboard
