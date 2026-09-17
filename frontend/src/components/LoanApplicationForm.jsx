import { useState } from 'react'
import { submitLoanApplication } from '../api/loansApi'

const initialValues = {
  loanAmount: '',
  assetValue: '',
  creditScore: '',
}

function LoanApplicationForm({ onApplicationSubmitted, onResult }) {
  const [values, setValues] = useState(initialValues)
  const [errors, setErrors] = useState({})
  const [submitError, setSubmitError] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  function handleChange(event) {
    const { name, value } = event.target
    setValues((currentValues) => ({ ...currentValues, [name]: value }))
    setErrors((currentErrors) => ({ ...currentErrors, [name]: '' }))
    setSubmitError('')
  }

  function validate() {
    const nextErrors = {}
    const loanAmount = Number(values.loanAmount)
    const assetValue = Number(values.assetValue)
    const creditScore = Number(values.creditScore)

    if (!Number.isFinite(loanAmount) || loanAmount <= 0) {
      nextErrors.loanAmount = 'Enter a loan amount greater than £0.'
    }

    if (!Number.isFinite(assetValue) || assetValue <= 0) {
      nextErrors.assetValue = 'Enter an asset value greater than £0.'
    }

    if (!Number.isInteger(creditScore) || creditScore < 1 || creditScore > 999) {
      nextErrors.creditScore = 'Enter a whole-number credit score from 1 to 999.'
    }

    return nextErrors
  }

  async function handleSubmit(event) {
    event.preventDefault()
    const nextErrors = validate()

    if (Object.keys(nextErrors).length > 0) {
      setErrors(nextErrors)
      return
    }

    setIsSubmitting(true)
    setSubmitError('')

    try {
      const result = await submitLoanApplication({
        loanAmount: Number(values.loanAmount),
        assetValue: Number(values.assetValue),
        creditScore: Number(values.creditScore),
      })
      onResult(result)
      await onApplicationSubmitted()
    } catch (error) {
      setSubmitError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="card form-card" onSubmit={handleSubmit} noValidate>
      <h3>Application details</h3>
      <p className="form-intro">All fields are required. Amounts should be entered in pounds sterling.</p>

      {submitError && <p className="form-error" role="alert">{submitError}</p>}

      <div className="field-grid">
        <div className="field">
          <label htmlFor="loanAmount">Loan amount</label>
          <div className="input-prefix">
            <span aria-hidden="true">£</span>
            <input
              id="loanAmount"
              name="loanAmount"
              type="number"
              min="0.01"
              step="0.01"
              inputMode="decimal"
              placeholder="e.g. 500,000"
              value={values.loanAmount}
              onChange={handleChange}
              aria-invalid={Boolean(errors.loanAmount)}
              aria-describedby={errors.loanAmount ? 'loanAmount-error' : undefined}
            />
          </div>
          {errors.loanAmount && <p className="field-error" id="loanAmount-error">{errors.loanAmount}</p>}
        </div>

        <div className="field">
          <label htmlFor="assetValue">Secured asset value</label>
          <div className="input-prefix">
            <span aria-hidden="true">£</span>
            <input
              id="assetValue"
              name="assetValue"
              type="number"
              min="0.01"
              step="0.01"
              inputMode="decimal"
              placeholder="e.g. 1,000,000"
              value={values.assetValue}
              onChange={handleChange}
              aria-invalid={Boolean(errors.assetValue)}
              aria-describedby={errors.assetValue ? 'assetValue-error' : undefined}
            />
          </div>
          {errors.assetValue && <p className="field-error" id="assetValue-error">{errors.assetValue}</p>}
        </div>
      </div>

      <div className="field">
        <label htmlFor="creditScore">Applicant credit score</label>
        <input
          id="creditScore"
          name="creditScore"
          type="number"
          min="1"
          max="999"
          step="1"
          inputMode="numeric"
          placeholder="1–999"
          value={values.creditScore}
          onChange={handleChange}
          aria-invalid={Boolean(errors.creditScore)}
          aria-describedby={errors.creditScore ? 'creditScore-error' : undefined}
        />
        {errors.creditScore && <p className="field-error" id="creditScore-error">{errors.creditScore}</p>}
      </div>

      <div className="form-actions">
        <button className="primary-button" type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Submitting application…' : 'Submit application'}
        </button>
        <small>The lending service determines the outcome.</small>
      </div>
    </form>
  )
}

export default LoanApplicationForm
